// Author: Gockner, Simon
// Created: 2019-06-07
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;

namespace LightweightIocContainer.Exceptions
{
    /// <summary>
    /// An error happened while trying to resolve a multiton
    /// </summary>
    internal class MultitonResolveException : InternalResolveException
    {
        /// <summary>
        /// An error happened while trying to resolve a multiton
        /// </summary>
        /// <param name="message">The exception message</param>
        /// <param name="type">The <see cref="System.Type"/> of the multiton that's responsible for the exception</param>
        public MultitonResolveException(string message, Type type)
            : base(message) =>
            Type = type;

        /// <summary>
        /// The <see cref="System.Type"/> of the multiton that's responsible for the exception
        /// </summary>
        public Type Type { get; }
    }
}