// Author: Simon Gockner
// Created: 2020-09-18
// Copyright(c) 2020 SimonG. All Rights Reserved.

using System;

namespace LightweightIocContainer.Exceptions
{
    public class GenericMethodNotFoundException : Exception
    {
        public GenericMethodNotFoundException(string functionName)
            : base($"Could not find function {functionName}")
        {
            
        }
    }
}