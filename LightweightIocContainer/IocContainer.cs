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
using LightweightIocContainer.Interfaces.Installers;
using LightweightIocContainer.Interfaces.Registrations;
using LightweightIocContainer.Registrations;

namespace LightweightIocContainer
{
    /// <summary>
    /// The main container that carries all the <see cref="IRegistrationBase"/>s and can resolve all the types you'll ever want
    /// </summary>
    public class IocContainer : IIocContainer
    {
        private readonly RegistrationFactory _registrationFactory;

        private readonly List<IRegistrationBase> _registrations = new List<IRegistrationBase>();
        private readonly List<(Type type, object instance)> _singletons = new List<(Type, object)>();
        private readonly List<(Type type, Type scope, ConditionalWeakTable<object, object> instances)> _multitons = new List<(Type, Type, ConditionalWeakTable<object, object>)>();


        /// <summary>
        /// The main container that carries all the <see cref="IRegistrationBase"/>s and can resolve all the types you'll ever want
        /// </summary>
        public IocContainer()
        {
            _registrationFactory = new RegistrationFactory(this);
        }


        /// <summary>
        /// Install the given installers for the current <see cref="IocContainer"/>
        /// </summary>
        /// <param name="installers">The given <see cref="IIocInstaller"/>s</param>
        /// <returns>An instance of the current <see cref="IocContainer"/></returns>
        public IIocContainer Install(params IIocInstaller[] installers)
        {
            foreach (IIocInstaller installer in installers)
            {
                installer.Install(this);
            }

            return this;
        }

        /// <summary>
        /// Register an Interface with a Type that implements it
        /// </summary>
        /// <typeparam name="TInterface">The Interface to register</typeparam>
        /// <typeparam name="TImplementation">The Type that implements the interface</typeparam>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IDefaultRegistration{TInterface}"/></param>
        /// <returns>The created <see cref="IRegistrationBase"/></returns>
        public IDefaultRegistration<TInterface> Register<TInterface, TImplementation>(Lifestyle lifestyle = Lifestyle.Transient) where TImplementation : TInterface
        {
            IDefaultRegistration<TInterface> registration = _registrationFactory.Register<TInterface, TImplementation>(lifestyle);
            Register(registration);

            return registration;
        }

        /// <summary>
        /// Register a <see cref="Type"/> without an interface
        /// </summary>
        /// <typeparam name="TImplementation">The <see cref="Type"/> to register</typeparam>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IDefaultRegistration{TInterface}"/></param>
        /// <returns>The created <see cref="IRegistrationBase"/></returns>
        public IDefaultRegistration<TImplementation> Register<TImplementation>(Lifestyle lifestyle = Lifestyle.Transient)
        {
            IDefaultRegistration<TImplementation> registration = _registrationFactory.Register<TImplementation>(lifestyle);
            Register(registration);

            return registration;
        }

        /// <summary>
        /// Register an Interface with a Type that implements it as a multiton
        /// </summary>
        /// <typeparam name="TInterface">The Interface to register</typeparam>
        /// <typeparam name="TImplementation">The Type that implements the interface</typeparam>
        /// <typeparam name="TScope">The Type of the multiton scope</typeparam>
        /// <returns>The created <see cref="IRegistrationBase"/></returns>
        public IMultitonRegistration<TInterface> Register<TInterface, TImplementation, TScope>() where TImplementation : TInterface
        {
            IMultitonRegistration<TInterface> registration = _registrationFactory.Register<TInterface, TImplementation, TScope>();
            Register(registration);

            return registration;
        }

        /// <summary>
        /// Register an Interface as an abstract typed factory
        /// </summary>
        /// <typeparam name="TFactory">The abstract typed factory to register</typeparam>
        /// <returns>The created <see cref="IRegistrationBase"/></returns>
        public ITypedFactoryRegistration<TFactory> RegisterFactory<TFactory>()
        {
            ITypedFactoryRegistration<TFactory> registration = _registrationFactory.RegisterFactory<TFactory>();
            Register(registration);

            return registration;
        }

        /// <summary>
        /// Register an Interface with an <see cref="ResolveCallback{T}"/> as a callback that is called when <see cref="Resolve{T}()"/> is called
        /// </summary>
        /// <typeparam name="TInterface">The Interface to register</typeparam>
        /// <param name="unitTestCallback">The <see cref="ResolveCallback{T}"/> for the callback</param>
        /// <returns>The created <see cref="IRegistrationBase"/></returns>
        public IUnitTestCallbackRegistration<TInterface> RegisterUnitTestCallback<TInterface>(ResolveCallback<TInterface> unitTestCallback)
        {
            IUnitTestCallbackRegistration<TInterface> registration = _registrationFactory.RegisterUnitTestCallback(unitTestCallback);
            Register(registration);

            return registration;
        }

        /// <summary>
        /// Add the <see cref="IRegistrationBase"/> to the the <see cref="IocContainer"/>
        /// </summary>
        /// <param name="registration">The given <see cref="IRegistrationBase"/></param>
        /// <exception cref="MultipleRegistrationException">The <see cref="Type"/> is already registered in this <see cref="IocContainer"/></exception>
        private void Register(IRegistrationBase registration)
        {
            //if type is already registered
            if (_registrations.Any(r => r.InterfaceType == registration.InterfaceType))
                throw new MultipleRegistrationException(registration.InterfaceType);

            _registrations.Add(registration);
        }

        /// <summary>
        /// Gets an instance of the given <see cref="Type"/>
        /// </summary>
        /// <typeparam name="T">The given <see cref="Type"/></typeparam>
        /// <returns>An instance of the given <see cref="Type"/></returns>
        public T Resolve<T>()
        {
            return ResolveInternal<T>(null);
        }

        /// <summary>
        /// Gets an instance of the given <see cref="Type"/>
        /// </summary>
        /// <typeparam name="T">The given <see cref="Type"/></typeparam>
        /// <param name="arguments">The constructor arguments</param>
        /// <returns>An instance of the given <see cref="Type"/></returns>
        public T Resolve<T>(params object[] arguments)
        {
            return ResolveInternal<T>(arguments);
        }

        /// <summary>
        /// Gets an instance of the given <see cref="Type"/>
        /// </summary>
        /// <param name="type">The given <see cref="Type"/></param>
        /// <param name="arguments">The constructor arguments</param>
        /// <param name="resolveStack">The current resolve stack</param>
        /// <returns>An instance of the given <see cref="Type"/></returns>
        /// <exception cref="InternalResolveException">Could not find function <see cref="ResolveInternal{T}"/></exception>
        private object Resolve(Type type, object[] arguments, List<Type> resolveStack)
        {
            MethodInfo resolveMethod = typeof(IocContainer).GetMethod(nameof(ResolveInternal), BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo genericResolveMethod = resolveMethod?.MakeGenericMethod(type);

            if (genericResolveMethod == null)
                throw new InternalResolveException($"Could not find function {nameof(ResolveInternal)}");

            try //exceptions thrown by methods called with invoke are wrapped into another exception, the exception thrown by the invoked method can be returned by `Exception.GetBaseException()`
            {
                return genericResolveMethod.Invoke(this, new object[] { arguments, resolveStack });
            }
            catch (Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

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
            IRegistrationBase registration = _registrations.FirstOrDefault(r => r.InterfaceType == typeof(T));
            if (registration == null)
                throw new TypeNotRegisteredException(typeof(T));

            //Circular dependency check
            if (resolveStack == null) //first resolve call
                resolveStack = new List<Type> {typeof(T)}; //create new stack and add the currently resolving type to the stack
            else if (resolveStack.Contains(typeof(T)))
                throw new CircularDependencyException(typeof(T), resolveStack); //currently resolving type is still resolving -> circular dependency
            else //not the first resolve call in chain but no circular dependencies for now
                resolveStack.Add(typeof(T)); //add currently resolving type to the stack

            T resolvedInstance;

            if (registration is IUnitTestCallbackRegistration<T> unitTestCallbackRegistration)
            {
                resolvedInstance = unitTestCallbackRegistration.UnitTestResolveCallback.Invoke(arguments);
            }
            else if (registration is IDefaultRegistration<T> defaultRegistration)
            {
                if (defaultRegistration.Lifestyle == Lifestyle.Singleton)
                    resolvedInstance = GetOrCreateSingletonInstance(defaultRegistration, arguments, resolveStack);
                else if (defaultRegistration is IMultitonRegistration<T> multitonRegistration && defaultRegistration.Lifestyle == Lifestyle.Multiton)
                    resolvedInstance = GetOrCreateMultitonInstance(multitonRegistration, arguments, resolveStack);
                else
                    resolvedInstance = CreateInstance(defaultRegistration, arguments, resolveStack);
            }
            else if (registration is ITypedFactoryRegistration<T> typedFactoryRegistration)
            {
                resolvedInstance = typedFactoryRegistration.Factory.Factory;
            }
            else
                throw new UnknownRegistrationException($"There is no registration of type {registration.GetType().Name}.");

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
        private T GetOrCreateSingletonInstance<T>(IDefaultRegistration<T> registration, object[] arguments, List<Type> resolveStack)
        {
            //if a singleton instance exists return it
            object instance = _singletons.FirstOrDefault(s => s.type == typeof(T)).instance;
            if (instance != null)
                return (T) instance;

            //if it doesn't already exist create a new instance and add it to the list
            T newInstance = CreateInstance(registration, arguments, resolveStack);
            _singletons.Add((typeof(T), newInstance));

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
        private T GetOrCreateMultitonInstance<T>(IMultitonRegistration<T> registration, object[] arguments, List<Type> resolveStack)
        {
            if (arguments == null || !arguments.Any())
                throw new MultitonResolveException("Can not resolve multiton without arguments.", typeof(T));

            object scopeArgument = arguments[0];
            if (scopeArgument.GetType() != registration.Scope && !registration.Scope.IsInstanceOfType(scopeArgument))
                throw new MultitonResolveException($"Can not resolve multiton without the first argument being the scope (should be of type {registration.Scope}).", typeof(T));

            //if a multiton for the given scope exists return it
            var instances = _multitons.FirstOrDefault(m => m.type == typeof(T) && m.scope == registration.Scope).instances; //get instances for the given type and scope
            if (instances != null)
            {
                if (instances.TryGetValue(scopeArgument, out object instance))
                    return (T) instance;

                T createdInstance = CreateInstance(registration, arguments, resolveStack);
                instances.Add(scopeArgument, createdInstance);

                return createdInstance;
            }

            T newInstance = CreateInstance(registration, arguments, resolveStack);

            ConditionalWeakTable<object, object> weakTable = new ConditionalWeakTable<object, object>();
            weakTable.Add(scopeArgument, newInstance);
            
            _multitons.Add((typeof(T), registration.Scope, weakTable));

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
        private T CreateInstance<T>(IDefaultRegistration<T> registration, object[] arguments, List<Type> resolveStack)
        {
            arguments = ResolveConstructorArguments(registration.ImplementationType, arguments, resolveStack);
            T instance = (T) Activator.CreateInstance(registration.ImplementationType, arguments);
            registration.OnCreateAction?.Invoke(instance); //TODO: Allow async OnCreateAction?

            return instance;
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
                    List<object> ctorParams = new List<object>();

                    ParameterInfo[] parameters = ctor.GetParameters();
                    foreach (ParameterInfo parameter in parameters)
                    {
                        object fittingArgument = new InternalResolvePlaceholder();
                        if (argumentsList != null)
                        {
                            fittingArgument = argumentsList.FirstOrGiven<object, InternalResolvePlaceholder>(a =>
                                a?.GetType() == parameter.ParameterType || parameter.ParameterType.IsInstanceOfType(a));
                            if (!(fittingArgument is InternalResolvePlaceholder))
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

                                    if (!(fittingArgument is InternalResolvePlaceholder))
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
                    if (noMatchingConstructorFoundException == null)
                        noMatchingConstructorFoundException = new NoMatchingConstructorFoundException(type);

                    noMatchingConstructorFoundException.AddInnerException(new ConstructorNotMatchingException(ctor, ex));
                }
            }

            if (noMatchingConstructorFoundException != null)
                throw noMatchingConstructorFoundException;
            
            return null;
        }

        /// <summary>
        /// Clear the multiton instances of the given <see cref="Type"/> from the registered multitons list
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> to clear the multiton instances</typeparam>
        public void ClearMultitonInstances<T>()
        {
            var multitonInstance = _multitons.FirstOrDefault(m => m.type == typeof(T));

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
        public bool IsTypeRegistered<T>() => _registrations.Any(registration => registration.InterfaceType == typeof(T));

        /// <summary>
        /// The <see cref="IDisposable.Dispose"/> method
        /// </summary>
        public void Dispose()
        {
            _registrations.Clear();
            _singletons.Clear();
            _multitons.Clear();
        }

        /// <summary>
        /// An internal placeholder that is used during the resolving process
        /// </summary>
        private class InternalResolvePlaceholder
        {

        }
    }
}