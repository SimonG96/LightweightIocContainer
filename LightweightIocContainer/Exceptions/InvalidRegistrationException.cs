// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;

namespace LightweightIocContainer.Exceptions
{
    /// <summary>
    /// The registration is not valid
    /// </summary>
    internal class InvalidRegistrationException : Exception
    {
        /// <summary>
        /// The registration is not valid
        /// </summary>
        /// <param name="message">The exception message</param>
        public InvalidRegistrationException(string message)
            : base(message)
        {

        }
    }
}