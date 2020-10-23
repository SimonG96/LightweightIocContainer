// Author: Simon Gockner
// Created: 2020-09-18
// Copyright(c) 2020 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Registrations
{
    /// <summary>
    /// <see cref="IRegistration"/> for open generic types
    /// </summary>
    public class OpenGenericRegistration : IOpenGenericRegistration
    {
        /// <summary>
        /// <see cref="IRegistration"/> for open generic types
        /// </summary>
        /// <param name="interfaceType">The <see cref="Type"/> of the interface</param>
        /// <param name="implementationType">The <see cref="Type"/> of the implementation type</param>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> of this <see cref="IOpenGenericRegistration"/></param>
        public OpenGenericRegistration(Type interfaceType, Type implementationType, Lifestyle lifestyle)
        {
            InterfaceType = interfaceType;
            ImplementationType = implementationType;
            Lifestyle = lifestyle;
            
            Name = $"{InterfaceType.Name}, {ImplementationType.Name}, Lifestyle: {Lifestyle.ToString()}";
        }
        
        /// <summary>
        /// The name of the <see cref="IRegistration"/>
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// The <see cref="Type"/> of the Interface that is registered with this <see cref="IRegistration"/>
        /// </summary>
        public Type InterfaceType { get; }
        
        /// <summary>
        /// The <see cref="Type"/> that implements the <see cref="IRegistration.InterfaceType"/> that is registered with this <see cref="IOpenGenericRegistration"/>
        /// </summary>
        public Type ImplementationType { get; }
        
        /// <summary>
        /// The Lifestyle of Instances that are created with this <see cref="IRegistration"/>
        /// </summary>
        public Lifestyle Lifestyle { get; }
    }
}