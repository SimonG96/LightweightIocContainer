// Author: Gockner, Simon
// Created: 2021-12-01
// Copyright(c) 2021 SimonG. All Rights Reserved.

using LightweightIocContainer.Interfaces.Factories;

namespace LightweightIocContainer.Factories
{
    /// <summary>
    /// <see cref="ITypedFactory"/> implementation for custom implemented factories
    /// </summary>
    public class CustomTypedFactory<TFactory> : TypedFactoryBase<TFactory>
    {
        
    }
}