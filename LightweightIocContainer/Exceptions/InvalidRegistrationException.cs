// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;

namespace LightweightIocContainer.Exceptions
{
    /// <summary>
    /// The registration is not valid
    /// </summary>
    public class InvalidRegistrationException : Exception
    {
        public InvalidRegistrationException(string message)
            : base(message)
        {

        }
    }
}