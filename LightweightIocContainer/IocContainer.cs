// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System.Collections.Concurrent;
using System.Reflection;
using LightweightIocContainer.Exceptions;
using LightweightIocContainer.Interfaces;
using LightweightIocContainer.Interfaces.Factories;
using LightweightIocContainer.Interfaces.Installers;
using LightweightIocContainer.Interfaces.Registrations;
using LightweightIocContainer.Interfaces.Registrations.Fluent;
using LightweightIocContainer.Registrations;
using LightweightIocContainer.ResolvePlaceholders;

namespace LightweightIocContainer;

/// <summary>
/// The main container that carries all the <see cref="IRegistration"/>s and can resolve all the types you'll ever want
/// </summary>
public class IocContainer : IIocContainer, IIocResolver
{
    private readonly RegistrationFactory _registrationFactory;

    private readonly ConcurrentDictionary<Type, object?> _singletons = [];
    private readonly ConcurrentDictionary<(Type type, Type scope), ConcurrentDictionary<object, object?>> _multitons = [];

    private readonly List<Type> _ignoreConstructorAttributes = [];

    /// <summary>
    /// The main container that carries all the <see cref="IRegistration"/>s and can resolve all the types you'll ever want
    /// </summary>
    public IocContainer() => _registrationFactory = new RegistrationFactory(this);

    internal List<IRegistration> Registrations { get; } = [];

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
    /// <returns>An instance of the given <see cref="Type"/></returns>
    public Task<T> ResolveAsync<T>() => ResolveInternalAsync<T>(null);

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
    /// <typeparam name="T">The given <see cref="Type"/></typeparam>
    /// <param name="arguments">The constructor arguments</param>
    /// <returns>An instance of the given <see cref="Type"/></returns>
    public Task<T> ResolveAsync<T>(params object[] arguments) => ResolveInstanceAsync<T>(arguments);

    /// <summary>
    /// Gets an instance of the given <see cref="Type"/> for a factory
    /// </summary>
    /// <typeparam name="T">The given <see cref="Type"/></typeparam>
    /// <param name="arguments">The constructor arguments</param>
    /// <returns>An instance of the given <see cref="Type"/></returns>
    public T FactoryResolve<T>(params object?[] arguments) => ResolveInternal<T>(arguments, null, true);
    
    /// <summary>
    /// Gets an instance of the given <see cref="Type"/> for a factory
    /// </summary>
    /// <typeparam name="T">The given <see cref="Type"/></typeparam>
    /// <param name="arguments">The constructor arguments</param>
    /// <returns>An instance of the given <see cref="Type"/></returns>
    public Task<T> FactoryResolveAsync<T>(params object?[] arguments) => ResolveInternalAsync<T>(arguments, null, true);

    /// <summary>
    /// Gets an instance of a given registered <see cref="Type"/>
    /// </summary>
    /// <typeparam name="T">The registered <see cref="Type"/></typeparam>
    /// <param name="arguments">The constructor arguments</param>
    /// <param name="resolveStack">The current resolve stack</param>
    /// <param name="isFactoryResolve">True if resolve is called from factory, false (default) if not</param>
    /// <returns>An instance of the given registered <see cref="Type"/></returns>
    private T ResolveInternal<T>(object?[]? arguments, List<Type>? resolveStack = null, bool isFactoryResolve = false)
    {
        (bool success, object resolvedObject, Exception? exception) = TryResolve<T>(arguments, resolveStack, isFactoryResolve);
        if (success)
            return ResolveInstance<T>(resolvedObject);

        if (exception is not null)
            throw exception;

        throw new Exception("Resolve Error");
    }
    
    private async Task<T> ResolveInternalAsync<T>(object?[]? arguments, List<Type>? resolveStack = null, bool isFactoryResolve = false)
    {
        (bool success, object resolvedObject, Exception? exception) = TryResolve<T>(arguments, resolveStack, isFactoryResolve);
        if (success)
            return await ResolveInstanceAsync<T>(resolvedObject);

        if (exception is not null)
            throw exception;

        throw new Exception("Resolve Error");
    }

    /// <summary>
    /// Tries to resolve the given <see cref="Type"/> with the given arguments
    /// </summary>
    /// <param name="arguments">The given arguments</param>
    /// <param name="resolveStack">The current resolve stack</param>
    /// <param name="isFactoryResolve">True if resolve is called from factory, false (default) if not</param>
    /// <typeparam name="T">The registered <see cref="Type"/></typeparam>
    /// <returns>An instance of the given registered <see cref="Type"/>, an <see cref="InternalToBeResolvedPlaceholder"/> if parameters need to be resolved or an <see cref="InternalFactoryMethodPlaceholder{T}"/> if a factory method is used to create an instance</returns>
    /// <exception cref="TypeNotRegisteredException">The given <see cref="Type"/> is not registered</exception>
    /// <exception cref="DirectResolveWithRegisteredFactoryNotAllowed">A direct resolve with a registered factory is not allowed</exception>
    /// <exception cref="InvalidRegistrationException">An interface was registered without an implementation or factory method</exception>
    /// <exception cref="MultitonResolveException">Tried resolving a multiton without scope argument</exception>
    /// <exception cref="NoMatchingConstructorFoundException">No matching constructor for the given <see cref="Type"/> found</exception>
    /// <exception cref="InternalResolveException">Getting resolve stack failed without exception</exception>
    private (bool success, object resolvedObject, Exception? exception) TryResolve<T>(object?[]? arguments, List<Type>? resolveStack, bool isFactoryResolve = false)
    {
        IRegistration? registration = FindRegistration<T>();
        if (registration == null)
            return (false, new object(), new TypeNotRegisteredException(typeof(T)));

        List<Type> internalResolveStack = resolveStack == null ? [] : [..resolveStack];
        (bool success, internalResolveStack, CircularDependencyException? circularDependencyException) = CheckForCircularDependencies<T>(internalResolveStack);
        
        if (!success && circularDependencyException is not null)
            return (false, new object(), circularDependencyException);
        
        if (!success)
            throw new Exception("Invalid return type!");

        object? existingInstance = TryGetExistingInstance<T>(registration, arguments);
        if (existingInstance != null)
            return (true, existingInstance, null);

        switch (registration)
        {
            case IWithFactoryInternal { Factory: not null } when !isFactoryResolve:
                return (false, new object(), new DirectResolveWithRegisteredFactoryNotAllowed(typeof(T)));
            case ISingleTypeRegistration<T> { InterfaceType.IsInterface: true, FactoryMethod: null } singleTypeRegistration:
                return (false, new object(), new InvalidRegistrationException($"Can't register an interface without its implementation type or without a factory method (Type: {singleTypeRegistration.InterfaceType})."));
            case ISingleTypeRegistration<T> { FactoryMethod: not null } singleTypeRegistration:
                return (true, new InternalFactoryMethodPlaceholder<T>(singleTypeRegistration), null);
        }

        if (registration is IWithParametersInternal { Parameters: not null } registrationWithParameters)
            arguments = UpdateArgumentsWithRegistrationParameters(registrationWithParameters, arguments);

        Type registeredType = GetType<T>(registration);
        (bool result, List<object?>? parametersToResolve, NoMatchingConstructorFoundException? exception) = 
            TryGetTypeResolveStack<T>(registeredType, arguments, internalResolveStack);

        if (registration is IMultitonRegistration multitonRegistration)
        {
            if (arguments == null || !arguments.Any())
                return (false, new object(), new MultitonResolveException("Can not resolve multiton without arguments.", registration.InterfaceType));

            object multitonScopeArgument = TryGetMultitonScopeArgument(multitonRegistration, arguments);

            parametersToResolve ??= [];
            parametersToResolve.Insert(0, multitonScopeArgument); //insert scope at first place, won't be passed to ctor when creating multiton
        }

        switch (result)
        {
            case true when registration is IOpenGenericRegistration openGenericRegistration:
            {
                Type genericImplementationType = openGenericRegistration.ImplementationType.MakeGenericType(typeof(T).GenericTypeArguments);
                return (true, new InternalToBeResolvedPlaceholder(genericImplementationType, registration, parametersToResolve), null);
            }
            case true:
                return (true, new InternalToBeResolvedPlaceholder(registeredType, registration, parametersToResolve), null);
        }

        if (exception != null)
            return (false, new object(), exception);

        return (false, new object(), new InternalResolveException("Getting resolve stack failed without exception."));
    }

    /// <summary>
    /// Tries to resolve the given <see cref="Type"/> with the given arguments without generic arguments
    /// </summary>
    /// <param name="type">The registered <see cref="Type"/></param>
    /// <param name="arguments">The given arguments</param>
    /// <param name="resolveStack">The current resolve stack</param>
    /// <param name="isFactoryResolve">True if resolve is called from factory, false (default) if not</param>
    /// <returns>An instance of the given registered <see cref="Type"/>, an <see cref="InternalToBeResolvedPlaceholder"/> if parameters need to be resolved or an <see cref="InternalFactoryMethodPlaceholder{T}"/> if a factory method is used to create an instance</returns>
    /// <exception cref="TypeNotRegisteredException">The given <see cref="Type"/> is not registered</exception>
    /// <exception cref="DirectResolveWithRegisteredFactoryNotAllowed">A direct resolve with a registered factory is not allowed</exception>
    /// <exception cref="InvalidRegistrationException">An interface was registered without an implementation or factory method</exception>
    /// <exception cref="MultitonResolveException">Tried resolving a multiton without scope argument</exception>
    /// <exception cref="NoMatchingConstructorFoundException">No matching constructor for the given <see cref="Type"/> found</exception>
    /// <exception cref="InternalResolveException">Getting resolve stack failed without exception</exception>
    internal (bool success, object resolvedObject, Exception? exception) TryResolveNonGeneric(Type type, object?[]? arguments, List<Type>? resolveStack, bool isFactoryResolve = false)
    {
        MethodInfo? method = typeof(IocContainer).GetMethod(nameof(TryResolve), BindingFlags.NonPublic | BindingFlags.Instance);
        MethodInfo? genericMethod = method?.MakeGenericMethod(type);

        if (genericMethod == null)
            throw new GenericMethodNotFoundException(nameof(TryResolve));
        
        object? resolvedValue = genericMethod.Invoke(this, [arguments, resolveStack, isFactoryResolve]);
        
        if (resolvedValue is not ValueTuple<bool, object, Exception?> resolvedTuple)
            throw new Exception("Invalid return value!");

        return resolvedTuple;
    }

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
            
        List<object?> parameters = [];
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

    private async Task<T> ResolvePlaceholderAsync<T>(InternalToBeResolvedPlaceholder toBeResolvedPlaceholder)
    {
        object? existingInstance = TryGetExistingInstance<T>(toBeResolvedPlaceholder.ResolvedRegistration, toBeResolvedPlaceholder.Parameters);
        if (existingInstance is T instance)
            return instance;

        if (toBeResolvedPlaceholder.Parameters == null)
            return await CreateInstanceAsync<T>(toBeResolvedPlaceholder.ResolvedRegistration, null);
            
        List<object?> parameters = [];
        foreach (object? parameter in toBeResolvedPlaceholder.Parameters)
        {
            if (parameter != null)
            {
                Type type = parameter is IInternalToBeResolvedPlaceholder internalToBeResolvedPlaceholder ?
                    internalToBeResolvedPlaceholder.ResolvedType : parameter.GetType();
                    
                parameters.Add(await ResolveInstanceNonGenericAsync(type, parameter));
            }
            else
                parameters.Add(parameter);
        }

        return await CreateInstanceAsync<T>(toBeResolvedPlaceholder.ResolvedRegistration, parameters.ToArray());
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
            InternalFactoryMethodPlaceholder<T> factoryMethodPlaceholder => CreateInstance<T>(factoryMethodPlaceholder.SingleTypeRegistration, null),
            _ => throw new InternalResolveException("Resolve returned wrong type.")
        };
    
    private async Task<T> ResolveInstanceAsync<T>(object resolvedObject) =>
        resolvedObject switch
        {
            T instance => instance,
            InternalToBeResolvedPlaceholder toBeResolvedPlaceholder => await ResolvePlaceholderAsync<T>(toBeResolvedPlaceholder),
            InternalFactoryMethodPlaceholder<T> factoryMethodPlaceholder => await CreateInstanceAsync<T>(factoryMethodPlaceholder.SingleTypeRegistration, null),
            _ => throw new InternalResolveException("Resolve returned wrong type.")
        };

    /// <summary>
    /// Resolve the given object instance without generic arguments
    /// </summary>
    /// <param name="type">The <see cref="Type"/> of the returned instance</param>
    /// <param name="resolvedObject">The given resolved object</param>
    /// <returns>An instance of the given resolved object</returns>
    /// <exception cref="InternalResolveException">Resolve returned wrong type</exception>
    private object? ResolveInstanceNonGeneric(Type type, object resolvedObject) => 
        GenericMethodCaller.CallPrivate(this, nameof(ResolveInstance), type, resolvedObject);
    
    private Task<object?> ResolveInstanceNonGenericAsync(Type type, object resolvedObject) => 
        GenericMethodCaller.CallPrivateAsync(this, nameof(ResolveInstance), type, resolvedObject);

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
            _singletons.TryAdd(GetType<T>(registration), instance);

        if (registration is IOnCreate onCreateRegistration && instance is not null)
            onCreateRegistration.OnCreateAction?.Invoke(instance);

        return instance;
    }

    private async Task<T> CreateInstanceAsync<T>(IRegistration registration, object?[]? arguments)
    {
        T instance = CreateInstance<T>(registration, arguments);
        if (registration is IOnCreate { OnCreateActionAsync: not null } onCreateRegistration && instance is not null)
            await onCreateRegistration.OnCreateActionAsync.Invoke(instance);

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
    private object? TryGetSingletonInstance<T>(IRegistration registration)
    {
        _singletons.TryGetValue(GetType<T>(registration), out object? value); //if a singleton instance exists return it
        return value;
    }

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
        bool foundMatchingMultitons = _multitons.TryGetValue((registration.ImplementationType, registration.Scope), out var matchingMultitons); //get instances for the given type and scope (use implementation type to resolve the correct instance for multiple multiton registrations as well)
        if (!foundMatchingMultitons || matchingMultitons is null)
            return null;

        return matchingMultitons.TryGetValue(scopeArgument, out object? instance) && instance != null ? instance : null;
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
        bool foundMatchingMultitons = _multitons.TryGetValue((registration.ImplementationType, registration.Scope), out var matchingMultitons);
        if (foundMatchingMultitons && matchingMultitons is not null)
        {
            T createdInstance = Creator.CreateInstance<T>(registration.ImplementationType, arguments[1..]);
            matchingMultitons.TryAdd(scopeArgument, createdInstance);

            return createdInstance;
        }

        T newInstance = Creator.CreateInstance<T>(registration.ImplementationType, arguments[1..]);
        
        ConcurrentDictionary<object,object?> concurrentDictionary = new();
        concurrentDictionary.TryAdd(scopeArgument, newInstance);
        
        _multitons.TryAdd((registration.ImplementationType, registration.Scope), concurrentDictionary);

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
            int argumentsSize = registration.Parameters.Count(p => p is not InternalResolvePlaceholder) + arguments.Length;
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
    private (bool result, List<object?>? parameters, NoMatchingConstructorFoundException? exception) TryGetTypeResolveStack<T>(Type type, IReadOnlyCollection<object?>? arguments, List<Type> resolveStack)
    {
        NoMatchingConstructorFoundException? noMatchingConstructorFoundException = null;
            
        //find best ctor
        List<ConstructorInfo> sortedConstructors = TryGetSortedConstructors(type);
        foreach (ConstructorInfo constructor in sortedConstructors)
        { 
            (bool result, List<object?>? parameters, List<ConstructorNotMatchingException>? exceptions) = TryGetConstructorResolveStack<T>(type, constructor, arguments, resolveStack);

            if (result)
                return (true, parameters, null);
                
            noMatchingConstructorFoundException ??= new NoMatchingConstructorFoundException(type);
            exceptions?.ForEach(e => noMatchingConstructorFoundException.AddInnerException(e));
        }

        return (false, null, noMatchingConstructorFoundException);
    }

    /// <summary>
    /// Try to get the resolve stack for a given constructor
    /// </summary>
    /// <param name="type">The <see cref="Type"/> that is currently getting resolved</param>
    /// <param name="constructor">The <see cref="ConstructorInfo"/> for the given constructor</param>
    /// <param name="arguments">The given arguments</param>
    /// <param name="resolveStack">The current resolve stack</param>
    /// <returns>
    /// <para>result: True if successful, false if not</para>
    /// <para>parameters: The parameters needed to resolve the given <see cref="Type"/></para>
    /// <para>exception: A List of <see cref="ConstructorNotMatchingException"/>s if the constructor is not matching</para>
    /// </returns>
    private (bool result, List<object?>? parameters, List<ConstructorNotMatchingException>? exceptions) TryGetConstructorResolveStack<T>(Type type, ConstructorInfo constructor, IReadOnlyCollection<object?>? arguments, List<Type> resolveStack)
    {
        List<ParameterInfo> constructorParameters = constructor.GetParameters().ToList();
            
        List<ConstructorNotMatchingException> exceptions = [];
        List<object?> parameters = [];

        List<object?>? passedArguments = null;
        if (arguments != null)
            passedArguments = [..arguments];

        foreach (ParameterInfo parameter in constructorParameters)
        {
            object? fittingArgument = new InternalResolvePlaceholder();
            if (passedArguments != null)
            {
                if (parameter.ParameterType.IsGenericParameter)
                {
                    Type? genericArgument = type.GetGenericArguments().FirstOrDefault(a => a.Name.Equals(parameter.ParameterType.Name));
                    if (genericArgument is not null)
                    {
                        Type genericArgumentType = typeof(T).GetGenericArguments()[genericArgument.GenericParameterPosition];
                        fittingArgument = passedArguments.FirstOrGiven<object?, InternalResolvePlaceholder>(a =>
                            a?.GetType() == genericArgumentType ||
                            genericArgumentType.IsInstanceOfType(a) ||
                            a is NullParameter nullParameter && genericArgumentType.IsAssignableFrom(nullParameter.ParameterType));
                    }
                }
                else
                {
                    fittingArgument = passedArguments.FirstOrGiven<object?, InternalResolvePlaceholder>(a =>
                        a?.GetType() == parameter.ParameterType ||
                        parameter.ParameterType.IsInstanceOfType(a) ||
                        a is NullParameter nullParameter && parameter.ParameterType.IsAssignableFrom(nullParameter.ParameterType));
                }
                
                if (fittingArgument is not InternalResolvePlaceholder)
                    passedArguments.Remove(fittingArgument);
                
                if (fittingArgument is NullParameter)
                    fittingArgument = null;
            }

            if (fittingArgument is InternalResolvePlaceholder)
            {
                (bool success, object? resolvedObject, Exception? exception) = TryResolveNonGeneric(parameter.ParameterType, null, resolveStack);
                if (success)
                    fittingArgument = resolvedObject;
                else if (!success && exception is not null)
                    exceptions.Add(new ConstructorNotMatchingException(constructor, exception));
                else
                    throw new Exception("Invalid return value!");

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

        if (passedArguments == null || !passedArguments.Any())
            return (!parameters.Any(p => p is InternalResolvePlaceholder), parameters, exceptions);
        
        exceptions.Add(new ConstructorNotMatchingException(constructor, new Exception("Not all given arguments were used!")));
        return (false, parameters, exceptions);
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
        List<ConstructorInfo> sortedConstructors = type.GetConstructors()
            .Where(c => !c.GetCustomAttributes().Any(a => _ignoreConstructorAttributes.Contains(a.GetType())))
            .OrderByDescending(c => c.GetParameters().Length)
            .ToList();
        
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
    private (bool success, List<Type> resolveStack, CircularDependencyException? exception) CheckForCircularDependencies<T>(List<Type>? resolveStack)
    {
        if (resolveStack == null) //first resolve call
            resolveStack = [typeof(T)]; //create new stack and add the currently resolving type to the stack
        else if (resolveStack.Contains(typeof(T)))
            return (false, [], new CircularDependencyException(typeof(T), resolveStack)); //currently resolving type is still resolving -> circular dependency
        else //not the first resolve call in chain but no circular dependencies for now
            resolveStack.Add(typeof(T)); //add currently resolving type to the stack
            
        return (true, resolveStack, null);
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
        
        _multitons.Remove((multitonRegistration.ImplementationType, multitonRegistration.Scope), out _);
    }

    /// <summary>
    /// Is the given <see cref="Type"/> registered with this <see cref="IocContainer"/>
    /// </summary>
    /// <typeparam name="T">The given <see cref="Type"/></typeparam>
    /// <returns>True if the given <see cref="Type"/> is registered with this <see cref="IocContainer"/>, false if not</returns>
    public bool IsTypeRegistered<T>() => FindRegistration<T>() != null;

    /// <summary>
    /// Register a custom <see cref="Attribute"/> that can annotate a constructor to be ignored
    /// </summary>
    /// <typeparam name="T">The custom <see cref="Attribute"/></typeparam>
    /// <exception cref="InvalidIgnoreConstructorAttributeException{T}">The passed <see cref="Attribute"/> can't be used on a constructor</exception>
    public void RegisterIgnoreConstructorAttribute<T>() where T : Attribute
    {
        AttributeUsageAttribute? attributeUsage = typeof(T).GetCustomAttribute<AttributeUsageAttribute>();
        if (attributeUsage == null || !attributeUsage.ValidOn.HasFlag(AttributeTargets.Constructor))
            throw new InvalidIgnoreConstructorAttributeException<T>();
        
        _ignoreConstructorAttributes.Add(typeof(T));
    }

    /// <summary>
    /// The <see cref="IDisposable.Dispose"/> method
    /// </summary>
    public void Dispose()
    {
        _singletons.Where(s => FindRegistration(s.Key) is IWithDisposeStrategyInternal {DisposeStrategy: DisposeStrategy.Container})
            .Select(s => s.Value)
            .OfType<IDisposable>()
            .ForEach(d => d.Dispose());

        _multitons.Where(m => FindRegistration(m.Key.type) is IWithDisposeStrategyInternal {DisposeStrategy: DisposeStrategy.Container})
            .SelectMany(m => m.Value)
            .Select(i => i.Value)
            .OfType<IDisposable>()
            .ForEach(d => d.Dispose());
            
        Registrations.Clear();
        _singletons.Clear();
        _multitons.Clear();
    }
}