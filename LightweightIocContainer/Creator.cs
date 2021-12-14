// Author: Gockner, Simon
// Created: 2021-12-14
// Copyright(c) 2021 SimonG. All Rights Reserved.

using System;

namespace LightweightIocContainer;

/// <summary>
/// Helper for dynamic instance creation
/// </summary>
internal static class Creator
{
    /// <summary>
    /// Creates an instance of the given <see cref="Type"/> with the given arguments
    /// </summary>
    /// <param name="type">The given <see cref="Type"/></param>
    /// <param name="arguments">The given arguments</param>
    /// <typeparam name="T">The <see cref="Type"/> that is returned</typeparam>
    /// <returns>A new instance of the given <see cref="Type"/></returns>
    /// <exception cref="InvalidOperationException">The given type could not be created</exception>
    public static T CreateInstance<T>(Type type, params object?[]? arguments) => 
        (T) (Activator.CreateInstance(type, arguments) ?? throw new InvalidOperationException($"Type {type} could not be created."));
}