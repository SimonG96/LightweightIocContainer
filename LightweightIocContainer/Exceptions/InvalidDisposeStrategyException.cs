// Author: Gockner, Simon
// Created: 2021-12-15
// Copyright(c) 2021 SimonG. All Rights Reserved.

namespace LightweightIocContainer.Exceptions;

/// <summary>
/// Invalid <see cref="DisposeStrategy"/> is used
/// </summary>
internal class InvalidDisposeStrategyException : IocContainerException
{
    /// <summary>
    /// Invalid <see cref="DisposeStrategy"/> is used
    /// </summary>
    /// <param name="disposeStrategy">The <see cref="DisposeStrategy"/></param>
    /// <param name="type">The <see cref="Type"/></param>
    /// <param name="lifestyle">The <see cref="Lifestyle"/></param>
    public InvalidDisposeStrategyException(DisposeStrategy disposeStrategy, Type type, Lifestyle lifestyle)
        : base($"Dispose strategy {disposeStrategy} is invalid for the type {type} and lifestyle {lifestyle}.")
    {
        
    }
}