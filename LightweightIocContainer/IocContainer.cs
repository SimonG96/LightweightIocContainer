// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using LightweightIocContainer.Exceptions;
using LightweightIocContainer.Interfaces;
using LightweightIocContainer.Interfaces.Factories;
using LightweightIocContainer.Interfaces.Installers;
using LightweightIocContainer.Interfaces.Registrations;
using LightweightIocContainer.Interfaces.Registrations.Fluent;
using LightweightIocContainer.Registrations;

namespace LightweightIocContainer
{
    /// <summary>
    /// The main container that carries all the <see cref="IRegistration"/>s and can resolve all the types you'll ever want
    /// </summary>
    public class IocContainer : IIocContainer, IResolver
    {
        private readonly RegistrationFactory _registrationFactory;

        private readonly List<(Type type, object instance)> _singletons = new();
        private readonly List<(Type type, Type scope, ConditionalWeakTable<object, object> instances)> _multitons = new();

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
                installer.Install(this);

            return this;
        }

        /// <summary>
        /// Register an Interface with a Type that implements it
        /// </summary>
        /// <typeparam name="TInterface">The Interface to register</typeparam>
        /// <typeparam name="TImplementation">The Type that implements the interface</typeparam>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IRegistrationBase"/></param>
        /// <returns>The created <see cref="IRegistration"/></returns>
        public ITypedRegistration<TInterface, TImplementation> Register<TInterface, TImplementation>(Lifestyle lifestyle = Lifestyle.Transient) where TImplementation : TInterface
        {
            ITypedRegistration<TInterface, TImplementation> registration = _registrationFactory.Register<TInterface, TImplementation>(lifestyle);
            Register(registration);

            return registration;
        }

        /// <summary>
        /// Register an open generic Interface with an open generic Type that implements it
        /// </summary>
        /// <param name="tInterface">The open generic Interface to register</param>
        /// <param name="tImplementation">The open generic Type that implements the interface</param>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IOpenGenericRegistration"/></param>
        /// <returns>The created <see cref="IRegistration"/></returns>
        /// <exception cref="InvalidRegistrationException">Function can only be used to register open generic types</exception>
        /// <exception cref="InvalidRegistrationException">Can't register a multiton with open generic registration</exception>
        public IOpenGenericRegistration RegisterOpenGenerics(Type tInterface, Type tImplementation, Lifestyle lifestyle = Lifestyle.Transient)
        {
            if (!tInterface.ContainsGenericParameters)
                throw new InvalidRegistrationException("This function can only be used to register open generic types.");
            
            if (lifestyle == Lifestyle.Multiton)
                throw new InvalidRegistrationException("Can't register a multiton with open generic registration."); //TODO: Is there any need for a possibility to register multitons with open generics?
            
            IOpenGenericRegistration registration = _registrationFactory.Register(tInterface, tImplementation, lifestyle);	
            Register(registration);	

            return registration;
        }

        /// <summary>
        /// Register multiple interfaces for a <see cref="Type"/> that implements them
        /// </summary>
        /// <typeparam name="TInterface1">The base interface to register</typeparam>
        /// <typeparam name="TInterface2">A second interface to register</typeparam>
        /// <typeparam name="TImplementation">The <see cref="Type"/> that implements both interfaces</typeparam>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IRegistrationBase"/></param>
        /// <returns>The created <see cref="IMultipleRegistration{TInterface1,TInterface2}"/></returns>
        public IMultipleRegistration<TInterface1, TInterface2, TImplementation> Register<TInterface1, TInterface2, TImplementation>(Lifestyle lifestyle = Lifestyle.Transient) where TImplementation : TInterface2, TInterface1
        {
            IMultipleRegistration<TInterface1, TInterface2, TImplementation> multipleRegistration = _registrationFactory.Register<TInterface1, TInterface2, TImplementation>(lifestyle);
            Register(multipleRegistration);

            return multipleRegistration;
        }

        /// <summary>
        /// Register multiple interfaces for a <see cref="Type"/> that implements them
        /// </summary>
        /// <typeparam name="TInterface1">The base interface to register</typeparam>
        /// <typeparam name="TInterface2">A second interface to register</typeparam>
        /// <typeparam name="TInterface3">A third interface to register</typeparam>
        /// <typeparam name="TImplementation">The <see cref="Type"/> that implements both interfaces</typeparam>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IRegistrationBase"/></param>
        /// <returns>The created <see cref="IMultipleRegistration{TInterface1,TInterface2,TInterface3}"/></returns>
        public IMultipleRegistration<TInterface1, TInterface2, TInterface3, TImplementation> Register<TInterface1, TInterface2, TInterface3, TImplementation>(Lifestyle lifestyle = Lifestyle.Transient) where TImplementation : TInterface3, TInterface2, TInterface1
        {
            IMultipleRegistration<TInterface1, TInterface2, TInterface3, TImplementation> multipleRegistration = _registrationFactory.Register<TInterface1, TInterface2, TInterface3, TImplementation>(lifestyle);
            Register(multipleRegistration);

            return multipleRegistration;
        }

        /// <summary>
        /// Register multiple interfaces for a <see cref="Type"/> that implements them
        /// </summary>
        /// <typeparam name="TInterface1">The base interface to register</typeparam>
        /// <typeparam name="TInterface2">A second interface to register</typeparam>
        /// <typeparam name="TInterface3">A third interface to register</typeparam>
        /// <typeparam name="TInterface4">A fourth interface to register</typeparam>
        /// <typeparam name="TImplementation">The <see cref="Type"/> that implements both interfaces</typeparam>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IRegistrationBase"/></param>
        /// <returns>The created <see cref="IMultipleRegistration{TInterface1,TInterface2,TInterface3,TInterface4}"/></returns>
        public IMultipleRegistration<TInterface1, TInterface2, TInterface3, TInterface4, TImplementation> Register<TInterface1, TInterface2, TInterface3, TInterface4, TImplementation>(Lifestyle lifestyle = Lifestyle.Transient) where TImplementation : TInterface4, TInterface3, TInterface2, TInterface1
        {
            IMultipleRegistration<TInterface1, TInterface2, TInterface3, TInterface4, TImplementation> multipleRegistration = _registrationFactory.Register<TInterface1, TInterface2, TInterface3, TInterface4, TImplementation>(lifestyle);
            Register(multipleRegistration);

            return multipleRegistration;
        }

        /// <summary>
        /// Register multiple interfaces for a <see cref="Type"/> that implements them
        /// </summary>
        /// <typeparam name="TInterface1">The base interface to register</typeparam>
        /// <typeparam name="TInterface2">A second interface to register</typeparam>
        /// <typeparam name="TInterface3">A third interface to register</typeparam>
        /// <typeparam name="TInterface4">A fourth interface to register</typeparam>
        /// <typeparam name="TInterface5">A fifth interface to register</typeparam>
        /// <typeparam name="TImplementation">The <see cref="Type"/> that implements both interfaces</typeparam>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IRegistrationBase"/></param>
        /// <returns>The created <see cref="IMultipleRegistration{TInterface1,TInterface2,TInterface3,TInterface4,TInterface5}"/></returns>
        public IMultipleRegistration<TInterface1, TInterface2, TInterface3, TInterface4, TInterface5, TImplementation> Register<TInterface1, TInterface2, TInterface3, TInterface4, TInterface5, TImplementation>(Lifestyle lifestyle = Lifestyle.Transient) where TImplementation : TInterface5, TInterface4, TInterface3, TInterface2, TInterface1
        {
            IMultipleRegistration<TInterface1, TInterface2, TInterface3, TInterface4, TInterface5, TImplementation> multipleRegistration = _registrationFactory.Register<TInterface1, TInterface2, TInterface3, TInterface4, TInterface5, TImplementation>(lifestyle);
            Register(multipleRegistration);

            return multipleRegistration;
        }

        /// <summary>
        /// Register a <see cref="Type"/> without an interface
        /// </summary>
        /// <typeparam name="TImplementation">The <see cref="Type"/> to register</typeparam>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IRegistrationBase"/></param>
        /// <returns>The created <see cref="IRegistration"/></returns>
        public ISingleTypeRegistration<TImplementation> Register<TImplementation>(Lifestyle lifestyle = Lifestyle.Transient)
        {
            ISingleTypeRegistration<TImplementation> registration = _registrationFactory.Register<TImplementation>(lifestyle);
            Register(registration);

            return registration;
        }

        /// <summary>
        /// Register an Interface with a Type that implements it as a multiton
        /// </summary>
        /// <typeparam name="TInterface">The Interface to register</typeparam>
        /// <typeparam name="TImplementation">The Type that implements the interface</typeparam>
        /// <typeparam name="TScope">The Type of the multiton scope</typeparam>
        /// <returns>The created <see cref="IRegistration"/></returns>
        public IMultitonRegistration<TInterface, TImplementation> RegisterMultiton<TInterface, TImplementation, TScope>() where TImplementation : TInterface
        {
            IMultitonRegistration<TInterface, TImplementation> registration = _registrationFactory.RegisterMultiton<TInterface, TImplementation, TScope>();
            Register(registration);

            return registration;
        }

        /// <summary>
        /// Register multiple interfaces for a <see cref="Type"/> that implements them as a multiton
        /// </summary>
        /// <typeparam name="TInterface1">The base interface to register</typeparam>
        /// <typeparam name="TInterface2">A second interface to register</typeparam>
        /// <typeparam name="TImplementation">The Type that implements the interface</typeparam>
        /// <typeparam name="TScope">The Type of the multiton scope</typeparam>
        /// <returns>The created <see cref="IRegistration"/></returns>
        public IMultipleMultitonRegistration<TInterface1, TInterface2, TImplementation> RegisterMultiton<TInterface1, TInterface2, TImplementation, TScope>() where TImplementation : TInterface1, TInterface2
        {
            IMultipleMultitonRegistration<TInterface1, TInterface2, TImplementation> registration = _registrationFactory.RegisterMultiton<TInterface1, TInterface2, TImplementation, TScope>();
            Register(registration);

            return registration;
        }

        /// <summary>
        /// Register an Interface as an abstract typed factory
        /// </summary>
        /// <typeparam name="TFactory">The abstract typed factory to register</typeparam>
        /// <returns>The created <see cref="IRegistration"/></returns>
        internal void RegisterFactory<TFactory>(ITypedFactory<TFactory> factory) => Register(_registrationFactory.RegisterFactory(factory));

        /// <summary>
        /// Add the <see cref="IRegistration"/> to the the <see cref="IocContainer"/>
        /// </summary>
        /// <param name="registration">The given <see cref="IRegistration"/></param>
        /// <exception cref="MultipleRegistrationException">The <see cref="Type"/> is already registered in this <see cref="IocContainer"/></exception>
        private void Register(IRegistration registration)
        {
            //if type is already registered
            if (Registrations.Any(r => r.InterfaceType == registration.InterfaceType))
                throw new MultipleRegistrationException(registration.InterfaceType);

            //don't allow lifestyle.multiton without iMultitonRegistration
            if (registration is ILifestyleProvider { Lifestyle: Lifestyle.Multiton } and not IMultitonRegistration)
                throw new InvalidRegistrationException("Can't register a type as Lifestyle.Multiton without a scope (Registration is not of type IMultitonRegistration).");

            Registrations.Add(registration);
        }

        /// <summary>
        /// Register all <see cref="IMultipleRegistration{TInterface1,TImplementation}.Registrations"/> from an <see cref="IMultipleRegistration{TInterface1,TImplementation}"/>
        /// </summary>
        /// <typeparam name="TInterface1">The <see cref="Type"/> of the first registered interface</typeparam>
        /// <typeparam name="TImplementation">The <see cref="Type"/> of the registered implementation</typeparam>
        /// <param name="multipleRegistration">The <see cref="IMultipleRegistration{TInterface1,TImplementation}"/></param>
        private void Register<TInterface1, TImplementation>(IMultipleRegistration<TInterface1, TImplementation> multipleRegistration) where TImplementation : TInterface1
        {
            foreach (IRegistration registration in multipleRegistration.Registrations)
                Register(registration);
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
        /// Gets an instance of the given <see cref="Type"/>
        /// </summary>
        /// <param name="type">The given <see cref="Type"/></param>
        /// <param name="arguments">The constructor arguments</param>
        /// <param name="resolveStack">The current resolve stack</param>
        /// <returns>An instance of the given <see cref="Type"/></returns>
        /// <exception cref="InternalResolveException">Could not find function <see cref="ResolveInternal{T}"/></exception>
        internal object Resolve(Type type, object[] arguments, List<Type> resolveStack) => 
            GenericMethodCaller.Call(this, nameof(ResolveInternal), type, BindingFlags.NonPublic | BindingFlags.Instance, arguments, resolveStack);

        /// <summary>
        /// Gets an instance of a given registered <see cref="Type"/>
        /// </summary>
        /// <typeparam name="T">The registered <see cref="Type"/></typeparam>
        /// <param name="arguments">The constructor arguments</param>
        /// <param name="resolveStack">The current resolve stack</param>
        /// <returns>An instance of the given registered <see cref="Type"/></returns>
        /// <exception cref="TypeNotRegisteredException">The given <see cref="Type"/> is not registered in this <see cref="IocContainer"/></exception>
        /// <exception cref="UnknownRegistrationException">The registration for the given <see cref="Type"/> has an unknown <see cref="Type"/></exception>
        private T ResolveInternal<T>(object[] arguments, List<Type> resolveStack = null)
        {
            IRegistration registration = FindRegistration<T>() ?? throw new TypeNotRegisteredException(typeof(T));

            //Circular dependency check
            if (resolveStack == null) //first resolve call
                resolveStack = new List<Type> {typeof(T)}; //create new stack and add the currently resolving type to the stack
            else if (resolveStack.Contains(typeof(T)))
                throw new CircularDependencyException(typeof(T), resolveStack); //currently resolving type is still resolving -> circular dependency
            else //not the first resolve call in chain but no circular dependencies for now
                resolveStack.Add(typeof(T)); //add currently resolving type to the stack

            T resolvedInstance = registration switch
            {
                RegistrationBase { Lifestyle: Lifestyle.Singleton } defaultRegistration => GetOrCreateSingletonInstance<T>(defaultRegistration, arguments, resolveStack),
                RegistrationBase { Lifestyle: Lifestyle.Multiton } and IMultitonRegistration multitonRegistration => GetOrCreateMultitonInstance<T>(multitonRegistration, arguments, resolveStack),
                RegistrationBase defaultRegistration => CreateInstance<T>(defaultRegistration, arguments, resolveStack),
                ITypedFactoryRegistration<T> typedFactoryRegistration => typedFactoryRegistration.Factory.Factory,
                _ => throw new UnknownRegistrationException($"There is no registration of type {registration.GetType().Name}.")
            };

            resolveStack.Remove(typeof(T)); //T was successfully resolved -> no circular dependency -> remove from resolve stack

            return resolvedInstance;
        }

        /// <summary>
        /// Gets or creates a singleton instance of a given <see cref="Type"/>
        /// </summary>
        /// <typeparam name="T">The given <see cref="Type"/></typeparam>
        /// <param name="registration">The registration of the given <see cref="Type"/></param>
        /// <param name="arguments">The arguments to resolve</param>
        /// <param name="resolveStack">The current resolve stack</param>
        /// <returns>An existing or newly created singleton instance of the given <see cref="Type"/></returns>
        private T GetOrCreateSingletonInstance<T>(IRegistration registration, object[] arguments, List<Type> resolveStack)
        {
            Type type = registration switch
            {
                ITypedRegistration typedRegistration => typedRegistration.ImplementationType,
                ISingleTypeRegistration<T> singleTypeRegistration => singleTypeRegistration.InterfaceType,
                _ => throw new UnknownRegistrationException($"There is no registration {registration.GetType().Name} that can have lifestyle singleton.")
            };

            //if a singleton instance exists return it
            object instance = _singletons.FirstOrDefault(s => s.type == type).instance;
            if (instance != null)
                return (T) instance;

            //if it doesn't already exist create a new instance and add it to the list
            T newInstance = CreateInstance<T>(registration, arguments, resolveStack);
            _singletons.Add((type, newInstance));

            return newInstance;
        }

        /// <summary>
        /// Gets or creates a multiton instance of a given <see cref="Type"/>
        /// </summary>
        /// <typeparam name="T">The given <see cref="Type"/></typeparam>
        /// <param name="registration">The registration of the given <see cref="Type"/></param>
        /// <param name="arguments">The arguments to resolve</param>
        /// <param name="resolveStack">The current resolve stack</param>
        /// <returns>An existing or newly created multiton instance of the given <see cref="Type"/></returns>
        /// <exception cref="MultitonResolveException">No arguments given</exception>
        /// <exception cref="MultitonResolveException">Scope argument not given</exception>
        private T GetOrCreateMultitonInstance<T>(IMultitonRegistration registration, object[] arguments, List<Type> resolveStack)
        {
            if (arguments == null || !arguments.Any())
                throw new MultitonResolveException("Can not resolve multiton without arguments.", typeof(T));

            object scopeArgument = arguments[0];
            if (scopeArgument.GetType() != registration.Scope && !registration.Scope.IsInstanceOfType(scopeArgument))
                throw new MultitonResolveException($"Can not resolve multiton without the first argument being the scope (should be of type {registration.Scope}).", typeof(T));

            //if a multiton for the given scope exists return it
            var instances = _multitons.FirstOrDefault(m => m.type == registration.ImplementationType && m.scope == registration.Scope).instances; //get instances for the given type and scope (use implementation type to resolve the correct instance for multiple multiton registrations as well)
            if (instances != null)
            {
                if (instances.TryGetValue(scopeArgument, out object instance))
                    return (T) instance;

                T createdInstance = CreateInstance<T>(registration, arguments, resolveStack);
                instances.Add(scopeArgument, createdInstance);

                return createdInstance;
            }

            T newInstance = CreateInstance<T>(registration, arguments, resolveStack);

            ConditionalWeakTable<object, object> weakTable = new();
            weakTable.Add(scopeArgument, newInstance);
            
            _multitons.Add((registration.ImplementationType, registration.Scope, weakTable));

            return newInstance;
        }

        /// <summary>
        /// Creates an instance of a given <see cref="Type"/>
        /// </summary>
        /// <typeparam name="T">The given <see cref="Type"/></typeparam>
        /// <param name="registration">The registration of the given <see cref="Type"/></param>
        /// <param name="arguments">The constructor arguments</param>
        /// <param name="resolveStack">The current resolve stack</param>
        /// <returns>A newly created instance of the given <see cref="Type"/></returns>
        private T CreateInstance<T>(IRegistration registration, object[] arguments, List<Type> resolveStack)
        {
            if (registration is IWithParametersInternal { Parameters: { } } registrationWithParameters)
                arguments = UpdateArgumentsWithRegistrationParameters(registrationWithParameters, arguments);

            T instance;
            if (registration is IOpenGenericRegistration openGenericRegistration)
            {
                arguments = ResolveConstructorArguments(openGenericRegistration.ImplementationType, arguments, resolveStack);
                
                //create generic implementation type from generic arguments of T
                Type genericImplementationType = openGenericRegistration.ImplementationType.MakeGenericType(typeof(T).GenericTypeArguments);

                instance = (T) Activator.CreateInstance(genericImplementationType, arguments);
            }
            else if (registration is ITypedRegistration defaultRegistration)
            {
                arguments = ResolveConstructorArguments(defaultRegistration.ImplementationType, arguments, resolveStack);
                instance = (T) Activator.CreateInstance(defaultRegistration.ImplementationType, arguments);
            }
            else if (registration is ISingleTypeRegistration<T> singleTypeRegistration)
            {
                if (singleTypeRegistration.InterfaceType.IsInterface && singleTypeRegistration.FactoryMethod == null)
                    throw new InvalidRegistrationException($"Can't register an interface without its implementation type or without a factory method (Type: {singleTypeRegistration.InterfaceType}).");

                if (singleTypeRegistration.FactoryMethod == null) //type registration without interface -> just create this type
                {
                    arguments = ResolveConstructorArguments(singleTypeRegistration.InterfaceType, arguments, resolveStack);
                    instance = (T)Activator.CreateInstance(singleTypeRegistration.InterfaceType, arguments);
                }
                else //factory method set to create the instance
                    instance = singleTypeRegistration.FactoryMethod(this);
            }
            else
                throw new UnknownRegistrationException($"There is no registration of type {registration.GetType().Name}.");

            if (registration is IOnCreate onCreateRegistration)
                onCreateRegistration.OnCreateAction?.Invoke(instance); //TODO: Allow async OnCreateAction?

            return instance;
        }

        /// <summary>
        /// Update the given arguments with the <see cref="IWithParametersInternal.Parameters"/> of the given <see cref="IRegistrationBase"/>
        /// </summary>
        /// <param name="registration">The <see cref="IRegistrationBase"/> of the given <see cref="Type"/></param>
        /// <param name="arguments">The constructor arguments</param>
        /// <returns>The argument list updated with the <see cref="IWithParametersInternal.Parameters"/></returns>
        private object[] UpdateArgumentsWithRegistrationParameters(IWithParametersInternal registration, object[] arguments)
        {
            if (arguments != null && arguments.Any()) //if more arguments were passed to resolve
            {
                int argumentsSize = registration.Parameters.Length + arguments.Length;
                object[] newArguments = new object[argumentsSize];

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

                    object firstArgument = arguments.FirstOrGiven<object, InternalResolvePlaceholder>(a => a is not InternalResolvePlaceholder); //find the first argument that is not a placeholder
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
        /// Resolve the missing constructor arguments
        /// </summary>
        /// <param name="type">The <see cref="Type"/> that will be created</param>
        /// <param name="arguments">The existing arguments</param>
        /// <param name="resolveStack">The current resolve stack</param>
        /// <returns>An array of all needed constructor arguments to create the <see cref="Type"/></returns>
        /// <exception cref="NoMatchingConstructorFoundException">No matching constructor was found for the given or resolvable arguments</exception>
        [CanBeNull]
        private object[] ResolveConstructorArguments(Type type, object[] arguments, List<Type> resolveStack)
        {
            //find best ctor
            List<ConstructorInfo> sortedConstructors = type.GetConstructors().OrderByDescending(c => c.GetParameters().Length).ToList();
            if (!sortedConstructors.Any()) //no public constructor available
                throw new NoPublicConstructorFoundException(type);

            NoMatchingConstructorFoundException noMatchingConstructorFoundException = null;

            foreach (ConstructorInfo ctor in sortedConstructors)
            {
                try
                {
                    List<object> argumentsList = arguments?.ToList();
                    List<object> ctorParams = new();

                    ParameterInfo[] parameters = ctor.GetParameters();
                    foreach (ParameterInfo parameter in parameters)
                    {
                        object fittingArgument = new InternalResolvePlaceholder();
                        if (argumentsList != null)
                        {
                            fittingArgument = argumentsList.FirstOrGiven<object, InternalResolvePlaceholder>(a =>
                                a?.GetType() == parameter.ParameterType || parameter.ParameterType.IsInstanceOfType(a));
                            
                            if (fittingArgument is not InternalResolvePlaceholder)
                            {
                                int index = argumentsList.IndexOf(fittingArgument);
                                argumentsList[index] = new InternalResolvePlaceholder();
                            }
                            else //fittingArgument is InternalResolvePlaceholder
                            {
                                try
                                {
                                    fittingArgument = Resolve(parameter.ParameterType, null, resolveStack);
                                }
                                catch (Exception)
                                {
                                    fittingArgument = argumentsList.FirstOrGiven<object, InternalResolvePlaceholder>(a => parameter.ParameterType.GetDefault() == a);

                                    if (fittingArgument is not InternalResolvePlaceholder)
                                    {
                                        int index = argumentsList.IndexOf(fittingArgument);
                                        argumentsList[index] = new InternalResolvePlaceholder();
                                    }
                                }
                            }
                        }

                        if (fittingArgument is InternalResolvePlaceholder && parameter.HasDefaultValue)
                            ctorParams.Add(parameter.DefaultValue);
                        else if (fittingArgument is InternalResolvePlaceholder)
                            ctorParams.Add(Resolve(parameter.ParameterType, null, resolveStack));
                        else
                            ctorParams.Add(fittingArgument);
                    }

                    return ctorParams.ToArray();
                }
                catch (CircularDependencyException) //don't handle circular dependencies as no matching constructor, just rethrow them
                {
                    throw;
                }
                catch (Exception ex)
                {
                    noMatchingConstructorFoundException ??= new NoMatchingConstructorFoundException(type);
                    noMatchingConstructorFoundException.AddInnerException(new ConstructorNotMatchingException(ctor, ex));
                }
            }

            if (noMatchingConstructorFoundException != null)
                throw noMatchingConstructorFoundException;
            
            return null;
        }

        [CanBeNull]
        private IRegistration FindRegistration<T>()
        {
            IRegistration registration = Registrations.FirstOrDefault(r => r.InterfaceType == typeof(T));
            if (registration != null)
                return registration;

            registration = Registrations.OfType<ITypedRegistration>().FirstOrDefault(r => r.ImplementationType == typeof(T));
            if (registration != null)
                return registration;
            
            //check for open generic registration
            if (!typeof(T).GenericTypeArguments.Any())
                return null;
            
            List<IRegistration> openGenericRegistrations = Registrations.Where(r => r.InterfaceType.ContainsGenericParameters).ToList();
            return !openGenericRegistrations.Any() ? null : openGenericRegistrations.FirstOrDefault(r => r.InterfaceType == typeof(T).GetGenericTypeDefinition());
        }
        
        /// <summary>
        /// Clear the multiton instances of the given <see cref="Type"/> from the registered multitons list
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> to clear the multiton instances</typeparam>
        public void ClearMultitonInstances<T>()
        {
            IRegistration registration = FindRegistration<T>();
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
            Registrations.Clear();
            _singletons.Clear();
            _multitons.Clear();
        }
    }
}