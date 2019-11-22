// Author: Simon Gockner
// Created: 2019-11-04
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;

namespace LightweightIocContainer.Exceptions
{
    /// <summary>
    /// No public constructor can be found for a <see cref="Type"/>
    /// </summary>
    internal class NoPublicConstructorFoundException : IocContainerException
    {
        /// <summary>
        /// No public constructor can be found for a <see cref="Type"/>
        /// </summary>
        /// <param name="type">The <see cref="Type"/> with no public constructor</param>
        public NoPublicConstructorFoundException(Type type)
            : base($"No public constructor for {type} found.")
        {
            Type = type;
        }

        /// <summary>
        /// The <see cref="Type"/> with no public constructor
        /// </summary>
        public Type Type { get; }
    }
}