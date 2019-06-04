// Author: simon.gockner
// Created: 2019-05-27
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Interfaces;

namespace LightweightIocContainer.Exceptions
{
    /// <summary>
    /// An internal Error happened while the <see cref="IInjectorContainer"/> tried to resolve an instance
    /// </summary>
    public class InternalResolveException : Exception
    {
        public InternalResolveException(string message)
            : base(message)
        {

        }
    }
}