// Author: Gockner, Simon
// Created: 2019-07-01
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;

namespace LightweightIocContainer;

internal static class TypeExtension
{
    /// <summary>
    /// Returns the default value for a given <see cref="Type"/>
    /// </summary>
    /// <param name="type">The given <see cref="Type"/></param>
    /// <returns>The default value for the given <see cref="Type"/></returns>
    public static object? GetDefault(this Type type) => type.IsValueType ? Activator.CreateInstance(type) : null;
}