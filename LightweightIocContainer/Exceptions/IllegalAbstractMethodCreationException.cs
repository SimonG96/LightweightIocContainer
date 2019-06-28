// // Author: Gockner, Simon
// // Created: 2019-06-28
// // Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using System.Reflection;

namespace LightweightIocContainer.Exceptions
{
    /// <summary>
    /// The creation of the abstract method is illegal in its current state
    /// </summary>
    public class IllegalAbstractMethodCreationException : Exception
    {
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