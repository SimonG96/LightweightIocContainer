// Author: simon.gockner
// Created: 2019-05-21
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Interfaces;

namespace LightweightIocContainer.Exceptions
{
    /// <summary>
    /// The Type is not registered in this <see cref="IInjectorContainer"/>
    /// </summary>
    public class TypeNotRegisteredException : Exception
    {
        public TypeNotRegisteredException(Type type)
            : base($"Type {type.Name} is not registered in this InjectorContainer.")
        {
            Type = type;
        }

        /// <summary>
        /// The unregistered Type
        /// </summary>
        public Type Type { get; }
    }
}