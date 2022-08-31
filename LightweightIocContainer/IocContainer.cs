// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using LightweightIocContainer.Exceptions;
using LightweightIocContainer.Interfaces;
using LightweightIocContainer.Interfaces.Factories;
using LightweightIocContainer.Interfaces.Installers;
using LightweightIocContainer.Interfaces.Registrations;
using LightweightIocContainer.Interfaces.Registrations.Fluent;
using LightweightIocContainer.Registrations;
using LightweightIocContainer.ResolvePlaceholders;

namespace LightweightIocContainer
{
    /// <summary>
    /// The main container that carries all the <see cref="IRegistration"/>s and can resolve all the types you'll ever want
    /// </summary>
    public class IocContainer : IIocContainer, IIocResolver
    {
        private readonly RegistrationFactory _registrationFactory;

        private readonly List<(Type type, object? instance)> _singletons = new();
        private readonly List<(Type type, Type scope, ConditionalWeakTable<object, object?> instances)> _multitons = new();

        /// <summary>
        /// The main container that carries all the <see cref="IRegistration"/>s and can resolve all the types you'll ever want
        /// </summary>
        public IocContainer()
        {
            _registrationFactory = new RegistrationFactory(this);
            Registrations = new List<IRegistration>();
        }

        internal List<IRegistration> Registrations { get; }

        /// <summary>
        /// Install the given installers for the current <see cref="IocContainer"/>
        /// </summary>
        /// <param name="installers">The given <see cref="IIocInstaller"/>s</param>
        /// <returns>An instance of the current <see cref="IocContainer"/></returns>
        public IIocContainer Install(params IIocInstaller[] installers)
        {
            foreach (IIocInstaller installer in installers)
            {
                RegistrationCollector registrationCollector = new(_registrationFactory);
                installer.Install(registrationCollector);
                
                registrationCollector.Registrations.ForEach(Register);
            }

            return this;
        }

        /// <summary>
        /// Register an <see cref="IRegistration"/> at this <see cref="IocContainer"/> 
        /// </summary>
        /// <param name="addRegistration">The <see cref="Func{T, TResult}"/> that creates an <see cref="IRegistration"/></param>
        public void Register(Func<IRegistrationCollector, IRegistration> addRegistration)
        {
            RegistrationCollector registrationCollector = new(_registrationFactory);
            addRegistration(registrationCollector);
            
            registrationCollector.Registrations.ForEach(Register);
        }

        /// <summary>
        /// Register an Interface as an abstract typed factory
        /// </summary>
        /// <typeparam name="TFactory">The abstract typed factory to register</typeparam>
        /// <returns>The created <see cref="IRegistration"/></returns>
        internal void RegisterFactory<TFactory>(ITypedFactory<TFactory> factory)
        {
            ITypedFactoryRegistration<TFactory> typedFactoryRegistration = _registrationFactory.RegisterFactory(factory);
            if (!Registrations.Contains(typedFactoryRegistration))
                Registrations.Add(typedFactoryRegistration);
        }

        /// <summary>
        /// Add the <see cref="IRegistration"/> to the the <see cref="IocContainer"/>
        /// </summary>
        /// <param name="registration">The given <see cref="IRegistration"/></param>
        /// <exception cref="MultipleRegistrationException">The <see cref="Type"/> is already registered in this <see cref="IocContainer"/></exception>
        private void Register(IRegistration registration)
        {
            IRegistration? sameTypeRegistration = Registrations.FirstOrDefault(r => r.InterfaceType == registration.InterfaceType);
            if (sameTypeRegistration != null && !registration.Equals(sameTypeRegistration)) //if type is already registered differently
                throw new MultipleRegistrationException(registration.InterfaceType);

            if (registration is IInternalValidationProvider validationProvider)
                validationProvider.Validate();
            
            Registrations.Add(registration);
        }

        /// <summary>
        /// Gets an instance of the given <see cref="Type"/>
        /// </summary>
        /// <typeparam name="T">The given <see cref="Type"/></typeparam>
        /// <returns>An instance of the given <see cref="Type"/></returns>
        public virtual T Resolve<T>() => ResolveInternal<T>(null);

        /// <summary>
        /// Gets an instance of the given <see cref="Type"/>
        /// </summary>
        /// <typeparam name="T">The given <see cref="Type"/></typeparam>
        /// <param name="arguments">The constructor arguments</param>
        /// <returns>An instance of the given <see cref="Type"/></returns>
        public T Resolve<T>(params object[] arguments) => ResolveInternal<T>(arguments);

        /// <summary>
        /// Gets an instance of the given <see cref="Type"/> for a factory
        /// </summary>
        /// <typeparam name="T">The given <see cref="Type"/></typeparam>
        /// <param name="arguments">The constructor arguments</param>
        /// <returns>An instance of the given <see cref="Type"/></returns>
        public T FactoryResolve<T>(params object[] arguments) => ResolveInternal<T>(arguments, null, true);

        /// <summary>
        /// Gets an instance of a given registered <see cref="Type"/>
        /// </summary>
        /// <typeparam name="T">The registered <see cref="Type"/></typeparam>
        /// <param name="arguments">The constructor arguments</param>
        /// <param name="resolveStack">The current resolve stack</param>
        /// <param name="isFactoryResolve">True if resolve is called from factory, false (default) if not</param>
        /// <returns>An instance of the given registered <see cref="Type"/></returns>
        private T ResolveInternal<T>(object[]? arguments, List<Type>? resolveStack = null, bool isFactoryResolve = false) => ResolveInstance<T>(TryResolve<T>(arguments, resolveStack, isFactoryResolve));

        /// <summary>
        /// Tries to resolve the given <see cref="Type"/> with the given arguments
        /// </summary>
        /// <param name="arguments">The given arguments</param>
        /// <param name="resolveStack">The current resolve stack</param>
        /// <param name="isFactoryResolve">True if resolve is called from factory, false (default) if not</param>
        /// <typeparam name="T">The registered <see cref="Type"/></typeparam>
        /// <returns>An instance of the given registered <see cref="Type"/>, an <see cref="InternalToBeResolvedPlaceholder"/> if parameters need to be resolved or an <see cref="InternalFactoryMethodPlaceholder{T}"/> if a factory method is used to create an instance</returns>
        /// <exception cref="TypeNotRegisteredException">The given <see cref="Type"/> is not registered</exception>
        /// <exception cref="InvalidRegistrationException">An interface was registered without an implementation or factory method</exception>
        /// <exception cref="MultitonResolveException">Tried resolving a multiton without scope argument</exception>
        /// <exception cref="NoMatchingConstructorFoundException">No matching constructor for the given <see cref="Type"/> found</exception>
        /// <exception cref="InternalResolveException">Getting resolve stack failed without exception</exception>
        private object TryResolve<T>(object?[]? arguments, List<Type>? resolveStack, bool isFactoryResolve = false)
        {
            IRegistration registration = FindRegistration<T>() ?? throw new TypeNotRegisteredException(typeof(T));

            List<Type> internalResolveStack = resolveStack == null ? new List<Type>() : new List<Type>(resolveStack);
            internalResolveStack = CheckForCircularDependencies<T>(internalResolveStack);

            object? existingInstance = TryGetExistingInstance<T>(registration, arguments);
            if (existingInstance != null)
                return existingInstance;

            switch (registration)
            {
                case IWithFactoryInternal { Factory: { } } when !isFactoryResolve:
                    throw new DirectResolveWithRegisteredFactoryNotAllowed(typeof(T));
                case ISingleTypeRegistration<T> singleTypeRegistration when singleTypeRegistration.InterfaceType.IsInterface && singleTypeRegistration.FactoryMethod == null:
                    throw new InvalidRegistrationException($"Can't register an interface without its implementation type or without a factory method (Type: {singleTypeRegistration.InterfaceType}).");
                case ISingleTypeRegistration<T> { FactoryMethod: { } } singleTypeRegistration:
                    return new InternalFactoryMethodPlaceholder<T>(singleTypeRegistration);
            }

            if (registration is IWithParametersInternal { Parameters: { } } registrationWithParameters)
                arguments = UpdateArgumentsWithRegistrationParameters(registrationWithParameters, arguments);

            Type registeredType = GetType<T>(registration);
            (bool result, List<object?>? parametersToResolve, NoMatchingConstructorFoundException? exception) = 
                TryGetTypeResolveStack(registeredType, arguments, internalResolveStack);

            if (registration is IMultitonRegistration multitonRegistration)
            {
                if (arguments == null || !arguments.Any())
                    throw new MultitonResolveException("Can not resolve multiton without arguments.", registration.InterfaceType);

                object multitonScopeArgument = TryGetMultitonScopeArgument(multitonRegistration, arguments);

                parametersToResolve ??= new List<object?>();
                parametersToResolve.Insert(0, multitonScopeArgument); //insert scope at first place, won't be passed to ctor when creating multiton
            }
            
            if (result) 
                return new InternalToBeResolvedPlaceholder(registeredType, registration, parametersToResolve);
            
            if (exception != null)
                throw exception;

            throw new InternalResolveException("Getting resolve stack failed without exception.");
        }

        /// <summary>
        /// Tries to resolve the given <see cref="Type"/> with the given arguments without generic arguments
        /// </summary>
        /// <param name="type">The registered <see cref="Type"/></param>
        /// <param name="arguments">The given arguments</param>
        /// <param name="resolveStack">The current resolve stack</param>
        /// <param name="isFactoryResolve"></param>
        /// <returns>An instance of the given registered <see cref="Type"/>, an <see cref="InternalToBeResolvedPlaceholder"/> if parameters need to be resolved or an <see cref="InternalFactoryMethodPlaceholder{T}"/> if a factory method is used to create an instance</returns>
        /// <exception cref="TypeNotRegisteredException">The given <see cref="Type"/> is not registered</exception>
        /// <exception cref="InvalidRegistrationException">An interface was registered without an implementation or factory method</exception>
        /// <exception cref="MultitonResolveException">Tried resolving a multiton without scope argument</exception>
        /// <exception cref="NoMatchingConstructorFoundException">No matching constructor for the given <see cref="Type"/> found</exception>
        /// <exception cref="InternalResolveException">Getting resolve stack failed without exception</exception>
        internal object? TryResolveNonGeneric(Type type, object?[]? arguments, List<Type>? resolveStack, bool isFactoryResolve = false) => 
            GenericMethodCaller.CallPrivate(this, nameof(TryResolve), type, arguments, resolveStack, isFactoryResolve);
        
        /// <summary>
        /// Recursively resolve a <see cref="Type"/> with the given parameters for an <see cref="InternalToBeResolvedPlaceholder"/>
        /// </summary>
        /// <param name="toBeResolvedPlaceholder">The <see cref="InternalToBeResolvedPlaceholder"/> that includes the type and resolve stack</param>
        /// <returns>A recursively resolved instance of the given <see cref="Type"/></returns>
        private T ResolvePlaceholder<T>(InternalToBeResolvedPlaceholder toBeResolvedPlaceholder)
        {
            object? existingInstance = TryGetExistingInstance<T>(toBeResolvedPlaceholder.ResolvedRegistration, toBeResolvedPlaceholder.Parameters);
            if (existingInstance is T instance)
                return instance;

            if (toBeResolvedPlaceholder.Parameters == null)
                return CreateInstance<T>(toBeResolvedPlaceholder.ResolvedRegistration, null);
            
            List<object?> parameters = new();
            foreach (object? parameter in toBeResolvedPlaceholder.Parameters)
            {
                if (parameter != null)
                {
                    Type type = parameter is IInternalToBeResolvedPlaceholder internalToBeResolvedPlaceholder ?
                        internalToBeResolvedPlaceholder.ResolvedType : parameter.GetType();
                    
                    parameters.Add(ResolveInstanceNonGeneric(type, parameter));
                }
                else
                    parameters.Add(parameter);
            }

            return CreateInstance<T>(toBeResolvedPlaceholder.ResolvedRegistration, parameters.ToArray());
        }

        /// <summary>
        /// Resolve the given object instance
        /// </summary>
        /// <param name="resolvedObject">The given resolved object</param>
        /// <typeparam name="T">The <see cref="Type"/> of the returned instance</typeparam>
        /// <returns>An instance of the given resolved object</returns>
        /// <exception cref="InternalResolveException">Resolve returned wrong type</exception>
        private T ResolveInstance<T>(object resolvedObject) =>
            resolvedObject switch
            {
                T instance => instance,
                InternalToBeResolvedPlaceholder toBeResolvedPlaceholder => ResolvePlaceholder<T>(toBeResolvedPlaceholder),
                InternalFactoryMethodPlaceholder<T> factoryMethodPlaceholder => factoryMethodPlaceholder.FactoryMethod(this),
                _ => throw new InternalResolveException("Resolve returned wrong type.")
            };

        /// <summary>
        /// Resolve the given object instance without generic arguments
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the returned instance</param>
        /// <param name="resolveObject"></param>
        /// <returns>An instance of the given resolved object</returns>
        /// <exception cref="InternalResolveException">Resolve returned wrong type</exception>
        private object? ResolveInstanceNonGeneric(Type type, object resolveObject) => 
            GenericMethodCaller.CallPrivate(this, nameof(ResolveInstance), type, resolveObject);

        /// <summary>
        /// Creates an instance of a given <see cref="Type"/>
        /// </summary>
        /// <typeparam name="T">The given <see cref="Type"/></typeparam>
        /// <param name="registration">The registration of the given <see cref="Type"/></param>
        /// <param name="arguments">The constructor arguments</param>
        /// <returns>A newly created instance of the given <see cref="Type"/></returns>
        private T CreateInstance<T>(IRegistration registration, object?[]? arguments)
        {
            T instance;
            if (registration is IOpenGenericRegistration openGenericRegistration)
            {
                //create generic implementation type from generic arguments of T
                Type genericImplementationType = openGenericRegistration.ImplementationType.MakeGenericType(typeof(T).GenericTypeArguments);
                instance = Creator.CreateInstance<T>(genericImplementationType, arguments);
            }
            else if (registration is ISingleTypeRegistration<T> singleTypeRegistration)
                instance = singleTypeRegistration.FactoryMethod == null ? Creator.CreateInstance<T>(singleTypeRegistration.InterfaceType, arguments) : singleTypeRegistration.FactoryMethod(this);
            else if (registration is ILifestyleProvider { Lifestyle: Lifestyle.Multiton } and IMultitonRegistration multitonRegistration)
                instance = CreateMultitonInstance<T>(multitonRegistration, arguments);
            else if (registration is ITypedRegistration defaultRegistration)
                instance = Creator.CreateInstance<T>(defaultRegistration.ImplementationType, arguments);
            else
                throw new UnknownRegistrationException($"There is no registration of type {registration.GetType().Name}.");

            if (registration is ILifestyleProvider { Lifestyle: Lifestyle.Singleton }) 
                _singletons.Add((GetType<T>(registration), instance));

            if (registration is IOnCreate onCreateRegistration)
                onCreateRegistration.OnCreateAction?.Invoke(instance); //TODO: Allow async OnCreateAction?

            return instance;
        }
        
        /// <summary>
        /// Try to get an already existing instance (factory, singleton, multiton)
        /// </summary>
        /// <param name="registration">The given <see cref="IRegistration"/></param>
        /// <param name="arguments">The given arguments</param>
        /// <typeparam name="T">The <see cref="Type"/> of the instance</typeparam>
        /// <returns>An already existing instance if possible, null if not</returns>
        private object? TryGetExistingInstance<T>(IRegistration registration, IReadOnlyList<object?>? arguments) =>
            registration switch
            {
                ITypedFactoryRegistration<T> typedFactoryRegistration => typedFactoryRegistration.Factory.Factory,
                ILifestyleProvider { Lifestyle: Lifestyle.Singleton } => TryGetSingletonInstance<T>(registration),
                ILifestyleProvider { Lifestyle: Lifestyle.Multiton } and IMultitonRegistration multitonRegistration => TryGetMultitonInstance(multitonRegistration, arguments),
                _ => null
            };
        
        /// <summary>
        /// Try to get an existing singleton instance for a given <see cref="IRegistration"/>
        /// </summary>
        /// <param name="registration">The <see cref="IRegistration"/></param>
        /// <returns>A singleton instance if existing for the given <see cref="IRegistration"/>, null if not</returns>
        private object? TryGetSingletonInstance<T>(IRegistration registration) => 
            _singletons.FirstOrDefault(s => s.type == GetType<T>(registration)).instance; //if a singleton instance exists return it

        /// <summary>
        /// Try to get an existing multiton instance for a given <see cref="IMultitonRegistration"/>
        /// </summary>
        /// <param name="registration">The given <see cref="IMultitonRegistration"/></param>
        /// <param name="arguments">The given arguments</param>
        /// <returns>A multiton instance if existing for the given <see cref="IRegistration"/>, null if not</returns>
        /// <exception cref="MultitonResolveException">Tried resolving a multiton without scope argument</exception>
        private object? TryGetMultitonInstance(IMultitonRegistration registration, IReadOnlyList<object?>? arguments)
        {
            if (arguments == null || !arguments.Any())
                throw new MultitonResolveException("Can not resolve multiton without arguments.", registration.InterfaceType);
            
            object scopeArgument = TryGetMultitonScopeArgument(registration, arguments);

            //if a multiton for the given scope exists return it
            var matchingMultitons = _multitons.FirstOrDefault(m => m.type == registration.ImplementationType && m.scope == registration.Scope); //get instances for the given type and scope (use implementation type to resolve the correct instance for multiple multiton registrations as well)
            if (matchingMultitons == default)
                return null;

            return matchingMultitons.instances.TryGetValue(scopeArgument, out object? instance) && instance != null ? instance : null;
        }

        /// <summary>
        /// Try to get the multiton scope argument for a given <see cref="IMultitonRegistration"/>
        /// </summary>
        /// <param name="registration">The given <see cref="IMultitonRegistration"/></param>
        /// <param name="arguments">The given arguments</param>
        /// <returns>The multiton scope argument for the given <see cref="IMultitonRegistration"/></returns>
        /// <exception cref="MultitonResolveException">Tried resolving a multiton without correct scope argument</exception>
        private object TryGetMultitonScopeArgument(IMultitonRegistration registration, IReadOnlyList<object?> arguments)
        {
            object? scopeArgument = arguments[0];
            if (scopeArgument?.GetType() != registration.Scope && !registration.Scope.IsInstanceOfType(scopeArgument))
                throw new MultitonResolveException($"Can not resolve multiton without the first argument being the scope (should be of type {registration.Scope}).", registration.InterfaceType);
            
            return scopeArgument;
        }
        
        /// <summary>
        /// Gets or creates a multiton instance of a given <see cref="Type"/>
        /// </summary>
        /// <typeparam name="T">The given <see cref="Type"/></typeparam>
        /// <param name="registration">The registration of the given <see cref="Type"/></param>
        /// <param name="arguments">The arguments to resolve</param>
        /// <returns>An existing or newly created multiton instance of the given <see cref="Type"/></returns>
        /// <exception cref="MultitonResolveException">No arguments given</exception>
        /// <exception cref="MultitonResolveException">Scope argument not given</exception>
        private T CreateMultitonInstance<T>(IMultitonRegistration registration, object?[]? arguments)
        {
            if (arguments == null || !arguments.Any())
                throw new MultitonResolveException("Can not resolve multiton without arguments.", registration.InterfaceType);
            
            object scopeArgument = TryGetMultitonScopeArgument(registration, arguments);

            //if a multiton for the given scope exists return it
            var matchingMultitons = _multitons.FirstOrDefault(m => m.type == registration.ImplementationType && m.scope == registration.Scope); //get instances for the given type and scope (use implementation type to resolve the correct instance for multiple multiton registrations as well)
            if (matchingMultitons != default)
            {
                T createdInstance = Creator.CreateInstance<T>(registration.ImplementationType, arguments[1..]);
                matchingMultitons.instances.Add(scopeArgument, createdInstance);

                return createdInstance;
            }

            T newInstance = Creator.CreateInstance<T>(registration.ImplementationType, arguments[1..]);
            _multitons.Add((registration.ImplementationType, registration.Scope, new ConditionalWeakTable<object, object?> { { scopeArgument, newInstance } }));

            return newInstance;
        }

        /// <summary>
        /// Update the given arguments with the <see cref="IWithParametersInternal.Parameters"/> of the given <see cref="IRegistrationBase"/>
        /// </summary>
        /// <param name="registration">The <see cref="IRegistrationBase"/> of the given <see cref="Type"/></param>
        /// <param name="arguments">The constructor arguments</param>
        /// <returns>The argument list updated with the <see cref="IWithParametersInternal.Parameters"/></returns>
        private object?[]? UpdateArgumentsWithRegistrationParameters(IWithParametersInternal registration, object?[]? arguments)
        {
            if (registration.Parameters == null)
                return arguments;
            
            if (arguments != null && arguments.Any()) //if more arguments were passed to resolve
            {
                int argumentsSize = registration.Parameters.Length + arguments.Length;
                object?[] newArguments = new object[argumentsSize];

                for (int i = 0; i < argumentsSize; i++)
                {
                    if (i < registration.Parameters.Length) //if `i` is bigger than the length of the parameters, take the given arguments
                    {
                        object currentParameter = registration.Parameters[i];
                        if (currentParameter is not InternalResolvePlaceholder) //use the parameter at the current index if it is not a placeholder
                        {
                            newArguments[i] = currentParameter;
                            continue;
                        }
                    }

                    object? firstArgument = arguments.FirstOrGiven<object?, InternalResolvePlaceholder>(a => a is not InternalResolvePlaceholder); //find the first argument that is not a placeholder
                    if (firstArgument is InternalResolvePlaceholder) //no more arguments available
                        break; //there won't be any more arguments

                    newArguments[i] = firstArgument;

                    int indexOfFirstArgument = Array.IndexOf(arguments, firstArgument);
                    arguments[indexOfFirstArgument] = new InternalResolvePlaceholder();
                }

                arguments = newArguments;
            }
            else //no more arguments were passed to resolve -> only use parameters set during registration
                arguments = registration.Parameters;

            return arguments;
        }

        /// <summary>
        /// Try to get the resolve stack for a given <see cref="Type"/>
        /// </summary>
        /// <param name="type">The given <see cref="Type"/></param>
        /// <param name="arguments">The given arguments</param>
        /// <param name="resolveStack">The current resolve stack</param>
        /// <returns>
        /// <para>result: True if successful, false if not</para>
        /// <para>parameters: The parameters needed to resolve the given <see cref="Type"/></para>
        /// <para>exception: A <see cref="NoMatchingConstructorFoundException"/> if no matching constructor was found</para>
        /// </returns>
        private (bool result, List<object?>? parameters, NoMatchingConstructorFoundException? exception) TryGetTypeResolveStack(Type type, object?[]? arguments, List<Type> resolveStack)
        {
            NoMatchingConstructorFoundException? noMatchingConstructorFoundException = null;
            
            //find best ctor
            List<ConstructorInfo> sortedConstructors = TryGetSortedConstructors(type);
            foreach (ConstructorInfo constructor in sortedConstructors)
            { 
                (bool result, List<object?>? parameters, List<ConstructorNotMatchingException>? exceptions) = TryGetConstructorResolveStack(constructor, arguments, resolveStack);

                if (result)
                    return (true, parameters, null);
                
                noMatchingConstructorFoundException ??= new NoMatchingConstructorFoundException(type);
                exceptions?.ForEach(e => 
                    noMatchingConstructorFoundException.AddInnerException(new ConstructorNotMatchingException(constructor, e)));
            }

            return (false, null, noMatchingConstructorFoundException);
        }

        /// <summary>
        /// Try to get the resolve stack for a given constructor
        /// </summary>
        /// <param name="constructor">The <see cref="ConstructorInfo"/> for the given constructor</param>
        /// <param name="arguments">The given arguments</param>
        /// <param name="resolveStack">The current resolve stack</param>
        /// <returns>
        /// <para>result: True if successful, false if not</para>
        /// <para>parameters: The parameters needed to resolve the given <see cref="Type"/></para>
        /// <para>exception: A List of <see cref="ConstructorNotMatchingException"/>s if the constructor is not matching</para>
        /// </returns>
        private (bool result, List<object?>? parameters, List<ConstructorNotMatchingException>? exceptions) TryGetConstructorResolveStack(ConstructorInfo constructor, object?[]? arguments, List<Type> resolveStack)
        {
            List<ParameterInfo> constructorParameters = constructor.GetParameters().ToList();
            if (!constructorParameters.Any())
                return (true, null, null);
            
            List<ConstructorNotMatchingException> exceptions = new();
            List<object?> parameters = new();

            List<object?>? passedArguments = null;
            if (arguments != null)
                passedArguments = new List<object?>(arguments);

            foreach (ParameterInfo parameter in constructorParameters)
            {
                object? fittingArgument = new InternalResolvePlaceholder();
                if (passedArguments != null)
                {
                    fittingArgument = passedArguments.FirstOrGiven<object?, InternalResolvePlaceholder>(a =>
                        a?.GetType() == parameter.ParameterType || parameter.ParameterType.IsInstanceOfType(a));
                    
                    if (fittingArgument is not InternalResolvePlaceholder)
                        passedArguments.Remove(fittingArgument);
                }

                if (fittingArgument is InternalResolvePlaceholder)
                {
                    try
                    {
                        fittingArgument = TryResolveNonGeneric(parameter.ParameterType, null, resolveStack);
                    }
                    catch (Exception exception)
                    {
                        exceptions.Add(new ConstructorNotMatchingException(constructor, exception));
                    }

                    if (fittingArgument is InternalResolvePlaceholder && passedArguments != null)
                    {
                        fittingArgument = passedArguments.FirstOrGiven<object?, InternalResolvePlaceholder>(a => parameter.ParameterType.GetDefault() == a);

                        if (fittingArgument is not InternalResolvePlaceholder)
                            passedArguments.Remove(fittingArgument);
                    }
                }

                if (fittingArgument is InternalResolvePlaceholder && parameter.HasDefaultValue)
                    parameters.Add(parameter.DefaultValue);
                else
                    parameters.Add(fittingArgument);
            }

            return (!parameters.Any(p => p is InternalResolvePlaceholder), parameters, exceptions);
        }

        /// <summary>
        /// Find the <see cref="IRegistration"/> for the given <see cref="Type"/>
        /// </summary>
        /// <typeparam name="T">The given <see cref="Type"/></typeparam>
        /// <returns>The <see cref="IRegistration"/> for the given <see cref="Type"/></returns>
        private IRegistration? FindRegistration<T>() => FindRegistration(typeof(T));

        /// <summary>
        /// Find the <see cref="IRegistration"/> for the given <see cref="Type"/>
        /// </summary>
        /// <param name="type">The given <see cref="Type"/></param>
        /// <returns>The <see cref="IRegistration"/> for the given <see cref="Type"/></returns>
        private IRegistration? FindRegistration(Type type)
        {
            IRegistration? registration = Registrations.FirstOrDefault(r => r.InterfaceType == type);
            if (registration != null)
                return registration;

            registration = Registrations.OfType<ITypedRegistration>().FirstOrDefault(r => r.ImplementationType == type);
            if (registration != null)
                return registration;
            
            //check for open generic registration
            if (!type.GenericTypeArguments.Any())
                return null;
            
            List<IRegistration> openGenericRegistrations = Registrations.Where(r => r.InterfaceType.ContainsGenericParameters).ToList();
            return !openGenericRegistrations.Any() ? null : openGenericRegistrations.FirstOrDefault(r => r.InterfaceType == type.GetGenericTypeDefinition());
        }
        
        /// <summary>
        /// Try to get the sorted constructors for the given <see cref="Type"/>
        /// </summary>
        /// <param name="type">The given <see cref="Type"/></param>
        /// <returns>A list of sorted <see cref="ConstructorInfo"/> for the given <see cref="Type"/></returns>
        /// <exception cref="NoPublicConstructorFoundException">No public constructor was found for the given <see cref="Type"/></exception>
        private List<ConstructorInfo> TryGetSortedConstructors(Type type)
        {
            List<ConstructorInfo> sortedConstructors = type.GetConstructors().OrderByDescending(c => c.GetParameters().Length).ToList();
            if (!sortedConstructors.Any()) //no public constructor available
                throw new NoPublicConstructorFoundException(type);
            
            return sortedConstructors;
        }
        
        /// <summary>
        /// Get the implementation type for the given <see cref="IRegistration"/>
        /// </summary>
        /// <param name="registration">The given <see cref="IRegistration"/></param>
        /// <typeparam name="T">The given <see cref="Type"/> of the interface</typeparam>
        /// <returns>The implementation <see cref="Type"/> for the given <see cref="IRegistration"/></returns>
        /// <exception cref="UnknownRegistrationException">Unknown <see cref="IRegistration"/> passed</exception>
        private Type GetType<T>(IRegistration registration) =>
            registration switch
            {
                ITypedRegistration typedRegistration => typedRegistration.ImplementationType,
                ISingleTypeRegistration<T> singleTypeRegistration => singleTypeRegistration.InterfaceType,
                _ => throw new UnknownRegistrationException($"Unknown registration used: {registration.GetType().Name}.")
            };
        
        /// <summary>
        /// Check the given resolve stack for circular dependencies
        /// </summary>
        /// <param name="resolveStack">The given resolve stack</param>
        /// <typeparam name="T">The given <see cref="Type"/></typeparam>
        /// <returns>The new resolve stack</returns>
        /// <exception cref="CircularDependencyException">A circular dependency was detected</exception>
        private List<Type> CheckForCircularDependencies<T>(List<Type>? resolveStack)
        {
            if (resolveStack == null) //first resolve call
                resolveStack = new List<Type> {typeof(T)}; //create new stack and add the currently resolving type to the stack
            else if (resolveStack.Contains(typeof(T)))
                throw new CircularDependencyException(typeof(T), resolveStack); //currently resolving type is still resolving -> circular dependency
            else //not the first resolve call in chain but no circular dependencies for now
                resolveStack.Add(typeof(T)); //add currently resolving type to the stack
            
            return resolveStack;
        }

        /// <summary>
        /// Clear the multiton instances of the given <see cref="Type"/> from the registered multitons list
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> to clear the multiton instances</typeparam>
        public void ClearMultitonInstances<T>()
        {
            IRegistration? registration = FindRegistration<T>();
            if (registration is not IMultitonRegistration multitonRegistration)
                return;
            
            var multitonInstance = _multitons.FirstOrDefault(m => m.type == multitonRegistration.ImplementationType);

            //it is allowed to clear a non existing multiton instance (don't throw an exception)
            if (multitonInstance == default)
                return;

            _multitons.Remove(multitonInstance);
        }

        /// <summary>
        /// Is the given <see cref="Type"/> registered with this <see cref="IocContainer"/>
        /// </summary>
        /// <typeparam name="T">The given <see cref="Type"/></typeparam>
        /// <returns>True if the given <see cref="Type"/> is registered with this <see cref="IocContainer"/>, false if not</returns>
        public bool IsTypeRegistered<T>() => FindRegistration<T>() != null;

        /// <summary>
        /// The <see cref="IDisposable.Dispose"/> method
        /// </summary>
        public void Dispose()
        {
            _singletons.Where(s => FindRegistration(s.type) is IWithDisposeStrategyInternal {DisposeStrategy: DisposeStrategy.Container})
                .Select(s => s.instance)
                .OfType<IDisposable>()
                .ForEach(d => d.Dispose());

            _multitons.Where(m => FindRegistration(m.type) is IWithDisposeStrategyInternal {DisposeStrategy: DisposeStrategy.Container})
                .SelectMany(m => m.instances)
                .Select(i => i.Value)
                .OfType<IDisposable>()
                .ForEach(d => d.Dispose());
            
            Registrations.Clear();
            _singletons.Clear();
            _multitons.Clear();
        }
    }
}