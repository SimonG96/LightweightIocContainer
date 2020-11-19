// Author: Gockner, Simon
// Created: 2019-11-04
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using System.Reflection;

namespace LightweightIocContainer.Exceptions
{
    /// <summary>
    /// The constructor does not match the given or resolvable arguments
    /// </summary>
    internal class ConstructorNotMatchingException : IocContainerException
    {
        /// <summary>
        /// The constructor does not match the given or resolvable arguments
        /// </summary>
        /// <param name="constructor">The constructor that does not match</param>
        /// <param name="exception">The inner exception</param>
        public ConstructorNotMatchingException(ConstructorInfo constructor, Exception exception)
            : base($"Constructor {constructor} does not match the given or resolvable arguments.", exception) =>
            Constructor = constructor;

        /// <summary>
        /// The constructor that does not match
        /// </summary>
        public ConstructorInfo Constructor { get; }
    }
}