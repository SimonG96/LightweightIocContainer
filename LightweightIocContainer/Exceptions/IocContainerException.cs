// Author: Gockner, Simon
// Created: 2019-11-22
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace LightweightIocContainer.Exceptions
{
    /// <summary>
    /// A base <see cref="Exception"/> for the <see cref="LightweightIocContainer"/>
    /// </summary>
    public abstract class IocContainerException : Exception
    {
        /// <summary>
        ///  A base <see cref="Exception"/> for the <see cref="LightweightIocContainer"/>
        /// </summary>
        protected IocContainerException() => InnerExceptions = new List<Exception>();

        /// <summary>
        /// A base <see cref="Exception"/> for the <see cref="LightweightIocContainer"/>
        /// </summary>
        /// <param name="message">The message of the <see cref="Exception"/></param>
        protected IocContainerException(string message)
            : base(message) =>
            InnerExceptions = new List<Exception>();

        /// <summary>
        /// A base <see cref="Exception"/> for the <see cref="LightweightIocContainer"/>
        /// </summary>
        /// <param name="message">The message of the <see cref="Exception"/></param>
        /// <param name="innerException">The inner <see cref="Exception"/></param>
        protected IocContainerException(string message, Exception innerException)
            : base(message, innerException) =>
            InnerExceptions = new List<Exception> {innerException};

        /// <summary>
        /// The inner exceptions of the <see cref="IocContainerException"/>
        /// </summary>
        [UsedImplicitly]
        public List<Exception> InnerExceptions { get; protected set; }
    }
}