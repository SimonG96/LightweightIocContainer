// // Author: Gockner, Simon
// // Created: 2019-06-07
// // Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Interfaces;

namespace LightweightIocContainer.Exceptions
{
    /// <summary>
    /// The Type is already registered in this <see cref="IIocContainer"/>
    /// </summary>
    public class MultipleRegistrationException : Exception
    {
        public MultipleRegistrationException(Type type)
            : base($"Type {type.Name} is already registered in this IocContainer.")
        {
            Type = type;
        }

        /// <summary>
        /// The registered Type
        /// </summary>
        public Type Type { get; }
    }
}