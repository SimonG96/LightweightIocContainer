// Author: Gockner, Simon
// Created: 2020-11-19
// Copyright(c) 2020 SimonG. All Rights Reserved.

using System;
using System.Collections.Generic;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Registrations
{
    public class MultipleMultitonRegistration<TInterface1, TInterface2, TImplementation> : MultitonRegistration<TInterface1, TImplementation>, IMultipleMultitonRegistration<TInterface1, TInterface2, TImplementation> where TImplementation : TInterface1, TInterface2
    {
        public MultipleMultitonRegistration(Type interfaceType1, Type interfaceType2, Type implementationType, Type scope)
            : base(interfaceType1, implementationType, scope)
        {
            Registrations = new List<IRegistration>()
            {
                new MultitonRegistration<TInterface1, TImplementation>(interfaceType1, implementationType, scope),
                new MultitonRegistration<TInterface2, TImplementation>(interfaceType2, implementationType, scope)
            };
        }

        public List<IRegistration> Registrations { get; }

        public override ITypedRegistrationBase<TInterface1, TImplementation> OnCreate(Action<TImplementation> action)
        {
            foreach (var registration in Registrations)
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