// // Author: Gockner, Simon
// // Created: 2019-06-07
// // Copyright(c) 2019 SimonG. All Rights Reserved.

using System;

namespace LightweightIocContainer.Exceptions
{
    /// <summary>
    /// An error happened while trying to resolve a multiton
    /// </summary>
    public class MultitonResolveException : InternalResolveException
    {
        public MultitonResolveException(string message, Type type)
            : base(message)
        {
            Type = type;
        }

        /// <summary>
        /// The type of the multiton that's responsible for the exception
        /// </summary>
        public Type Type { get; }
    }
}