// Author: Gockner, Simon
// Created: 2019-12-11
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;

namespace LightweightIocContainer;

internal static class ActionExtension
{
    /// <summary>
    /// Convert an <see cref="Action{T2}"/> to an <see cref="Action{T1}"/> of an inherited <see cref="Type"/>
    /// </summary>
    /// <typeparam name="T1">The <see cref="Type"/> of the <see cref="Action{T1}"/> to convert to, has to be implemented by <typeparamref name="T2"/></typeparam>
    /// <typeparam name="T2">The <see cref="Type"/> of the given <see cref="Action{T2}"/>, has to implement <typeparamref name="T1"/></typeparam>
    /// <param name="action">The given <see cref="Action{T2}"/> to convert</param>
    /// <returns>An <see cref="Action{T1}"/> converted from the given <see cref="Action{T2}"/></returns>
    public static Action<T1>? Convert<T1, T2>(this Action<T2>? action) where T1 : T2
    {
        if (action == null)
            return null;

        return t => action(t);
    }
}