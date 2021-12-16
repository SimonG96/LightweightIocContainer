// Author: Gockner, Simon
// Created: 2020-11-19
// Copyright(c) 2020 SimonG. All Rights Reserved.

using System;
using System.Collections.Generic;
using LightweightIocContainer.Interfaces;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Registrations
{
    /// <summary>
    /// An <see cref="IRegistrationBase"/> to register multiple interfaces for on implementation type that implements them as a multiton
    /// </summary>
    /// <typeparam name="TInterface1">The first interface</typeparam>
    /// <typeparam name="TInterface2">The second interface</typeparam>
    /// <typeparam name="TImplementation">The implementation</typeparam>
    internal class MultipleMultitonRegistration<TInterface1, TInterface2, TImplementation> : MultitonRegistration<TInterface1, TImplementation>, IMultipleMultitonRegistration<TInterface1, TInterface2, TImplementation> where TImplementation : TInterface1, TInterface2
    {
        /// <summary>
        /// An <see cref="IRegistrationBase"/> to register multiple interfaces for on implementation type that implements them as a multiton
        /// </summary>
        /// <param name="interfaceType1">The <see cref="Type"/> of the first interface</param>
        /// <param name="interfaceType2">The <see cref="Type"/> of the second interface</param>
        /// <param name="implementationType">The <see cref="Type"/> of the implementation</param>
        /// <param name="scope">The <see cref="Type"/> of the multiton scope</param>
        /// <param name="container">The current instance of the <see cref="IIocContainer"/></param>
        public MultipleMultitonRegistration(Type interfaceType1, Type interfaceType2, Type implementationType, Type scope, IocContainer container)
            : base(interfaceType1, implementationType, scope, container)
        {
            Registrations = new List<IRegistration>
            {
                new MultitonRegistration<TInterface1, TImplementation>(interfaceType1, implementationType, scope, container),
                new MultitonRegistration<TInterface2, TImplementation>(interfaceType2, implementationType, scope, container)
            };
        }

        /// <summary>
        /// A <see cref="List{T}"/> of <see cref="IRegistration"/>s that are registered within this <see cref="IMultipleRegistration{TInterface1,TImplementation}"/>
        /// </summary>
        public List<IRegistration> Registrations { get; }

        /// <summary>
        /// Pass an <see cref="Action{T}"/> that will be invoked when an instance of this type is created
        /// </summary>
        /// <param name="action">The <see cref="Action{T}"/></param>
        /// <returns>The current instance of this <see cref="ITypedRegistration{TInterface,TImplementation}"/></returns>
        public override ITypedRegistration<TInterface1, TImplementation> OnCreate(Action<TImplementation?> action)
        {
            foreach (IRegistration registration in Registrations)
            {
                if (registration is IMultitonRegistration<TInterface2, TImplementation> interface2Registration)
                    interface2Registration.OnCreate(action);
                else if (registration is IMultitonRegistration<TInterface1, TImplementation> interface1Registration)
                    interface1Registration.OnCreate(action);
            }

            return this;
        }
    }
}