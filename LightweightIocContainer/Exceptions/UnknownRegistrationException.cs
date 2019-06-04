﻿// Author: simon.gockner
// Created: 2019-05-21
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Exceptions
{
    /// <summary>
    /// An unknown <see cref="IRegistrationBase"/> was used
    /// </summary>
    public class UnknownRegistrationException : Exception
    {
        public UnknownRegistrationException(string message)
            : base(message)
        {

        }
    }
}