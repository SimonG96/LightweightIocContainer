// Author: Gockner, Simon
// Created: 2019-11-05
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using LightweightIocContainer.Interfaces;

namespace LightweightIocContainer.Exceptions
{
    /// <summary>
    /// A circular dependency was detected during <see cref="IIocContainer.Resolve{T}()"/>
    /// </summary>
    internal class CircularDependencyException : Exception
    {
        /// <summary>
        /// A circular dependency was detected during <see cref="IIocContainer.Resolve{T}()"/>
        /// </summary>
        /// <param name="resolvingType">The currently resolving <see cref="Type"/></param>
        /// <param name="resolveStack">The resolve stack at the time the <see cref="CircularDependencyException"/> was thrown</param>
        public CircularDependencyException(Type resolvingType, List<Type> resolveStack)
        {
            ResolvingType = resolvingType;
            ResolveStack = resolveStack;
        }


        /// <summary>
        /// The currently resolving <see cref="Type"/>
        /// </summary>
        public Type ResolvingType { get; }

        /// <summary>
        /// The resolve stack at the time the <see cref="CircularDependencyException"/> was thrown
        /// </summary>
        public List<Type> ResolveStack { get; }

        /// <summary>
        /// The exception message
        /// </summary>
        public override string Message
        {
            get
            {
                string message = $"Circular dependency has been detected when trying to resolve `{ResolvingType}`.\n" +
                                 "Resolve stack that resulted in the circular dependency:\n" +
                                 $"\t`{ResolvingType}` resolved as dependency of\n";

                if (ResolveStack == null || !ResolveStack.Any())
                    return message;

                for (int i = ResolveStack.Count - 1; i >= 1 ; i--)
                {
                    message += $"\t`{ResolveStack[i]}` resolved as dependency of\n";
                }

                message += $"\t`{ResolveStack[0]}` which is the root type being resolved.";
                return message;
            }
        }
    }
}