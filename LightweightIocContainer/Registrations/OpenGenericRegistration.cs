// Author: Simon Gockner
// Created: 2020-09-18
// Copyright(c) 2020 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Registrations
{
    public class OpenGenericRegistration : IOpenGenericRegistration
    {
        public OpenGenericRegistration(Type interfaceType, Type implementationType, Lifestyle lifestyle)
        {
            InterfaceType = interfaceType;
            ImplementationType = implementationType;
            Lifestyle = lifestyle;
            
            Name = $"{InterfaceType.Name}, {ImplementationType.Name}, Lifestyle: {Lifestyle.ToString()}";
        }
        
        public string Name { get; }
        public Type InterfaceType { get; }
        public Type ImplementationType { get; }
        public Lifestyle Lifestyle { get; }
    }
}