// Author: Gockner, Simon
// Created: 2021-12-06
// Copyright(c) 2021 SimonG. All Rights Reserved.

using System;

namespace LightweightIocContainer.Interfaces
{
    /// <summary>
    /// Provides <see cref="Resolve{T}()"/> methods
    /// </summary>
    public interface IResolver
    {
        /// <summary>
        /// Gets an instance of the given <see cref="Type"/>
        /// </summary>
        /// <typeparam name="T">The given <see cref="Type"/></typeparam>
        /// <returns>An instance of the given <see cref="Type"/></returns>
        T Resolve<T>();

        /// <summary>
        /// Gets an instance of the given <see cref="Type"/>
        /// </summary>
        /// <typeparam name="T">The given <see cref="Type"/></typeparam>
        /// <param name="arguments">The constructor arguments</param>
        /// <returns>An instance of the given <see cref="Type"/></returns>
        T Resolve<T>(params object[] arguments);
    }
}