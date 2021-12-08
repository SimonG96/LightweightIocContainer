// Author: Gockner, Simon
// Created: 2021-12-08
// Copyright(c) 2021 SimonG. All Rights Reserved.

using System;
using System.Collections.Generic;

namespace LightweightIocContainer.ResolvePlaceholders
{
    /// <summary>
    /// An internal placeholder that is used to hold types that need to be resolved during the resolving process
    /// </summary>
    internal class InternalToBeResolvedPlaceholder
    {
        public InternalToBeResolvedPlaceholder(Type resolvedType, List<object> parameters)
        {
            ResolvedType = resolvedType;
            Parameters = parameters;
        }

        public Type ResolvedType { get; }
        public List<object> Parameters { get; }
    }
}