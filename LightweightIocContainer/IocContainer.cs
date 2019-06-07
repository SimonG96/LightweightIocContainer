// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LightweightIocContainer.Exceptions;
using LightweightIocContainer.Interfaces;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer
{
    /// <summary>
    /// The main container that carries all the <see cref="IRegistrationBase"/>s and can resolve all the types you'll ever want
    /// </summary>
    public class IocContainer : IIocContainer
    {
        private readonly List<IRegistrationBase> _registrations = new List<IRegistrationBase>();
        private readonly List<(Type type, object instance)> _singletons = new List<(Type, object)>(); //TODO: Think about the usage of ConditionalWeakTable<>

        /// <summary>
        /// Install the given installers for the current <see cref="IocContainer"/>
        /// </summary>
        /// <param name="installers">The given <see cref="IIocInstaller"/>s</param>
        /// <returns>An instance of the current <see cref="IocContainer"/></returns>
        public IIocContainer Install(params IIocInstaller[] installers)
        {
            foreach (var installer in installers)
            {
                installer.Install(this);
            }

            return this;
        }

        /// <summary>
        /// Add the <see cref="IRegistrationBase"/> to the the <see cref="IocContainer"/>
        /// </summary>
        /// <param name="registration">The given <see cref="IRegistrationBase"/></param>
        /// <exception cref="MultipleRegistrationException">The Type is already registered in this <see cref="IocContainer"/></exception>
        public void Register(IRegistrationBase registration)
        {
            //if type is already registered
            if (_registrations.Any(r => r.InterfaceType == registration.InterfaceType))
                throw new MultipleRegistrationException(registration.InterfaceType);

            _registrations.Add(registration);
        }

        /// <summary>
        /// Gets an instance of the given type
        /// </summary>
        /// <typeparam name="T">The given type</typeparam>
        /// <returns>An instance of the given type</returns>
        public T Resolve<T>()
        {
            return ResolveInternal<T>(null);
        }

        /// <summary>
        /// Gets an instance of the given type
        /// </summary>
        /// <typeparam name="T">The given type</typeparam>
        /// <param name="arguments">The constructor arguments</param>
        /// <returns>An instance of the given type</returns>
        public T Resolve<T>(params object[] arguments)
        {
            return ResolveInternal<T>(arguments);
        }

        /// <summary>
        /// Gets an instance of the given type
        /// </summary>
        /// <param name="type">The given type</param>
        /// <param name="arguments">The constructor arguments</param>
        /// <returns>An instance of the given type</returns>
        /// <exception cref="InternalResolveException">Could not find function <see cref="ResolveInternal{T}"/></exception>
        public object Resolve(Type type, object[] arguments) //somehow the order of the arguments is different in the application compared to the unit test
        {
            var resolveMethod = typeof(IocContainer).GetMethod(nameof(ResolveInternal), BindingFlags.NonPublic | BindingFlags.Instance);
            var genericResolveMethod = resolveMethod?.MakeGenericMethod(type);

            if (genericResolveMethod == null)
                throw new InternalResolveException($"Could not find function {nameof(ResolveInternal)}");

            return genericResolveMethod.Invoke(this, new object[] {arguments});
        }

        /// <summary>
        /// Gets an instance of a given registered type
        /// </summary>
        /// <typeparam name="T">The registered type</typeparam>
        /// <param name="arguments">The constructor arguments</param>
        /// <returns>An instance of the given registered type</returns>
        /// <exception cref="TypeNotRegisteredException">The given type is not registered in this <see cref="IocContainer"/></exception>
        /// <exception cref="UnknownRegistrationException">The registration for the given type has an unknown type</exception>
        private T ResolveInternal<T>(params object[] arguments)
        {
            IRegistrationBase registration = _registrations.FirstOrDefault(r => r.InterfaceType == typeof(T));
            if (registration == null)
                throw new TypeNotRegisteredException(typeof(T));

            if (registration is IDefaultRegistration<T> defaultRegistration)
            {
                if (defaultRegistration.Lifestyle == Lifestyle.Singleton)
                    return GetOrCreateSingletonInstance<T>(defaultRegistration, arguments);

                return CreateInstance<T>(defaultRegistration, arguments);
            }
            else if (registration is ITypedFactoryRegistration<T> typedFactoryRegistration)
            {
                return typedFactoryRegistration.Factory.Factory;
            }
            else
                throw new UnknownRegistrationException($"There is no registration of type {registration.GetType().Name}.");
        }

        /// <summary>
        /// Gets or creates a singleton instance of a given type
        /// </summary>
        /// <typeparam name="T">The given type</typeparam>
        /// <param name="registration">The registration of the given type</param>
        /// <param name="arguments">The constructor arguments</param>
        /// <returns>An existing or newly created singleton instance of the given type</returns>
        private T GetOrCreateSingletonInstance<T>(IDefaultRegistration<T> registration, params object[] arguments)
        {
            //if a singleton instance exists return it
            object instance = _singletons.FirstOrDefault(s => s.type == typeof(T)).instance;
            if (instance != null)
                return (T) instance;

            //if it doesn't already exist create a new instance and add it to the list
            T newInstance = CreateInstance<T>(registration, arguments);
            _singletons.Add((typeof(T), newInstance));

            return newInstance;
        }

        /// <summary>
        /// Creates an instance of a given type
        /// </summary>
        /// <typeparam name="T">The given type</typeparam>
        /// <param name="registration">The registration of the given type</param>
        /// <param name="arguments">The constructor arguments</param>
        /// <returns>A newly created instance of the given type</returns>
        private T CreateInstance<T>(IDefaultRegistration<T> registration, params object[] arguments)
        {
            arguments = ResolveConstructorArguments(registration.ImplementationType, arguments);
            T instance = (T) Activator.CreateInstance(registration.ImplementationType, arguments);
            registration.OnCreateAction?.Invoke(instance); //TODO: Allow async OnCreateAction?

            return instance;
        }

        /// <summary>
        /// Resolve the missing constructor arguments
        /// </summary>
        /// <param name="type">The type that will be created</param>
        /// <param name="arguments">The existing arguments</param>
        /// <returns>An array of all needed constructor arguments to create <param name="type"></param></returns>
        private object[] ResolveConstructorArguments(Type type, object[] arguments)
        {
            //find best ctor
            var sortedCtors = type.GetConstructors().OrderByDescending(c => c.GetParameters().Length);
            foreach (var ctor in sortedCtors)
            {
                try
                {
                    List<object> argumentsList = arguments?.ToList();
                    List<object> ctorParams = new List<object>();

                    var parameters = ctor.GetParameters();
                    foreach (var parameter in parameters)
                    {
                        object fittingArgument = null;
                        if (argumentsList != null)
                        {
                            fittingArgument = argumentsList.FirstOrDefault(a => a?.GetType() == parameter.ParameterType);
                            if (fittingArgument != null)
                            {
                                int index = argumentsList.IndexOf(fittingArgument);
                                argumentsList[index] = null;
                            }
                        }

                        if (fittingArgument == null)
                            ctorParams.Add(Resolve(parameter.ParameterType, null));
                        else
                            ctorParams.Add(fittingArgument);
                    }

                    return ctorParams.ToArray();
                }
                catch (Exception ex) //TODO: Decide what exactly to do in this case
                {
                    continue;
                }
            }
            
            return null;
        }
        
        public void Dispose()
        {
            _registrations.Clear();
            _singletons.Clear();
        }
    }
}