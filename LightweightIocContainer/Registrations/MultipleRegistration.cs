// Author: Simon Gockner
// Created: 2019-12-07
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using System.Collections.Generic;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Registrations
{
    /// <summary>
    /// An <see cref="IRegistrationBase{TInterface}"/> to register multiple interfaces for on implementation type
    /// </summary>
    /// <typeparam name="TInterface1">The first interface</typeparam>
    /// <typeparam name="TInterface2">The second interface</typeparam>
    public class MultipleRegistration<TInterface1, TInterface2> : TypedRegistrationBase<TInterface1>, IMultipleRegistration<TInterface1, TInterface2>
    {
        /// <summary>
        /// An <see cref="IRegistrationBase{TInterface}"/> to register multiple interfaces for on implementation type
        /// </summary>
        /// <param name="interfaceType1">The <see cref="Type"/> of the first interface</param>
        /// <param name="interfaceType2">The <see cref="Type"/> of the second interface</param>
        /// <param name="implementationType">The <see cref="Type"/> of the implementation</param>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> of this <see cref="MultipleRegistration{TInterface1,TInterface2}"/></param>
        public MultipleRegistration(Type interfaceType1, Type interfaceType2, Type implementationType, Lifestyle lifestyle)
            : base(interfaceType1, implementationType, lifestyle)
        {
            Registrations = new List<IRegistration>()
            {
                new DefaultRegistration<TInterface1>(interfaceType1, implementationType, lifestyle),
                new DefaultRegistration<TInterface2>(interfaceType2, implementationType, lifestyle)
            };
        }

        /// <summary>
        /// A <see cref="List{T}"/> of <see cref="IRegistration"/>s that are registered within this <see cref="MultipleRegistration{TInterface1,TInterface2}"/>
        /// </summary>
        public List<IRegistration> Registrations { get; }

        /// <summary>
        /// Pass an <see cref="Action{T}"/> for each interface that will be invoked when instances of the types are created
        /// </summary>
        /// <param name="action1">The first <see cref="Action{T}"/></param>
        /// <param name="action2">The second <see cref="Action{T}"/></param>
        /// <returns>The current instance of this <see cref="MultipleRegistration{TInterface1,TInterface2}"/></returns>
        public IMultipleRegistration<TInterface1, TInterface2> OnCreate(Action<TInterface1> action1, Action<TInterface2> action2)
        {
            foreach (var registration in Registrations)
            {
                if (registration is IDefaultRegistration<TInterface2> interface2Registration)
                    interface2Registration.OnCreate(action2);
                else if (registration is IDefaultRegistration<TInterface1> interface1Registration)
                    interface1Registration.OnCreate(action1);
            }

            return this;
        }
    }
}