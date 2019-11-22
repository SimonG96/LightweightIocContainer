// Author: simon.gockner
// Created: 2019-05-21
// Copyright(c) 2019 SimonG. All Rights Reserved.

using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Exceptions
{
    /// <summary>
    /// An unknown <see cref="IRegistration"/> was used
    /// </summary>
    internal class UnknownRegistrationException : IocContainerException
    {
        /// <summary>
        /// An unknown <see cref="IRegistration"/> was used
        /// </summary>
        /// <param name="message">The exception message</param>
        public UnknownRegistrationException(string message)
            : base(message)
        {

        }
    }
}