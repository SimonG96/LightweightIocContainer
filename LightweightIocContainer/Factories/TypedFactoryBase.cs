// Author: Gockner, Simon
// Created: 2021-12-03
// Copyright(c) 2021 SimonG. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LightweightIocContainer.Exceptions;
using LightweightIocContainer.Interfaces.Factories;

namespace LightweightIocContainer.Factories;

/// <summary>
/// Base class for the <see cref="ITypedFactory"/>
/// </summary>
public abstract class TypedFactoryBase<TFactory> : ITypedFactory
{
    /// <summary>
    /// The create methods of this <see cref="ITypedFactory"/>
    /// </summary>
    public List<MethodInfo> CreateMethods
    {
        get
        {
            Type factoryType = typeof(TFactory);

            List<MethodInfo> createMethods = factoryType.GetMethods().Where(m => m.ReturnType != typeof(void)).ToList();
            if (!createMethods.Any())
                throw new InvalidFactoryRegistrationException($"Factory {factoryType.Name} has no create methods.");

            return createMethods;
        }
    }
}