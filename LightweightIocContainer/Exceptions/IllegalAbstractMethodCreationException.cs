// Author: Gockner, Simon
// Created: 2019-06-28
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using System.Reflection;

namespace LightweightIocContainer.Exceptions
{
    /// <summary>
    /// The creation of the abstract method is illegal in its current state
    /// </summary>
    internal class IllegalAbstractMethodCreationException : Exception
    {
        /// <summary>
        /// The creation of the abstract method is illegal in its current state
        /// </summary>
        /// <param name="message">The exception message</param>
        /// <param name="method">The method that is illegal to create</param>
        public IllegalAbstractMethodCreationException(string message, MethodInfo method)
            : base(message)
        {
            Method = method;
        }

        /// <summary>
        /// The Method whose creation is illegal
        /// </summary>
        public MethodInfo Method { get; }
    }
}