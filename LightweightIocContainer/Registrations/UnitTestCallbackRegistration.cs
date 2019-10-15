// Author: Gockner, Simon
// Created: 2019-10-15
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Interfaces;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Registrations
{
    /// <summary>
    /// A special <see cref="IRegistrationBase"/> that allows to set a <see cref="ResolveCallback{T}"/> as a callback that is called on <see cref="IIocContainer.Resolve{T}()"/>
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    public class UnitTestCallbackRegistration<TInterface> : IUnitTestCallbackRegistration<TInterface>
    {
        /// <summary>
        /// A special <see cref="IRegistrationBase"/> that allows to set a <see cref="ResolveCallback{T}"/> as a callback that is called on <see cref="IIocContainer.Resolve{T}()"/>
        /// </summary>
        /// <param name="interfaceType">The <see cref="Type"/> of the interface</param>
        /// <param name="unitTestResolveCallback">The <see cref="ResolveCallback{T}"/> that is set as a callback</param>
        public UnitTestCallbackRegistration(Type interfaceType, ResolveCallback<TInterface> unitTestResolveCallback)
        {
            InterfaceType = interfaceType;
            UnitTestResolveCallback = unitTestResolveCallback;

            Name = InterfaceType.Name;
        }

        /// <summary>
        /// The name of the <see cref="UnitTestCallbackRegistration{TInterface}"/>
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The <see cref="Type"/> of the Interface that is registered with this <see cref="UnitTestCallbackRegistration{TInterface}"/>
        /// </summary>
        public Type InterfaceType { get; }

        /// <summary>
        /// An <see cref="ResolveCallback{T}"/> that is set as a callback that is called on <see cref="IIocContainer.Resolve{T}()"/>
        /// </summary>
        public ResolveCallback<TInterface> UnitTestResolveCallback { get; }
    }
}