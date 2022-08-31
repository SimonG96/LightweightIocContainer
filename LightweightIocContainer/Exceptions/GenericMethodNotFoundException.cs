// Author: Simon Gockner
// Created: 2020-09-18
// Copyright(c) 2020 SimonG. All Rights Reserved.

namespace LightweightIocContainer.Exceptions;

/// <summary>
/// Could not find generic method
/// </summary>
internal class GenericMethodNotFoundException : Exception
{
    /// <summary>
    /// Could not find generic method
    /// </summary>
    /// <param name="functionName">The name of the generic method</param>
    public GenericMethodNotFoundException(string functionName)
        : base($"Could not find function {functionName}")
    {
            
    }
}