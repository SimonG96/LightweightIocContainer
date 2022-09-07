// Author: Gockner, Simon
// Created: 2022-09-06
// Copyright(c) 2022 SimonG. All Rights Reserved.

namespace LightweightIocContainer.Exceptions;

/// <summary>
/// The passed <see cref="Attribute"/> can't be used on a constructor
/// </summary>
/// <typeparam name="T">The used <see cref="Attribute"/></typeparam>
public class InvalidIgnoreConstructorAttributeException<T> : IocContainerException
{
    /// <summary>
    /// The passed <see cref="Attribute"/> can't be used on a constructor
    /// </summary>
    public InvalidIgnoreConstructorAttributeException()
        : base($"The passed Attribute ({typeof(T)}) can't be used on a constructor.")
    {
        
    }
}