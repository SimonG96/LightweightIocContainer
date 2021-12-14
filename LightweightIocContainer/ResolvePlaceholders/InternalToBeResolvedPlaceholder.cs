// Author: Gockner, Simon
// Created: 2021-12-08
// Copyright(c) 2021 SimonG. All Rights Reserved.

using System;
using System.Collections.Generic;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.ResolvePlaceholders
{
    /// <summary>
    /// An internal placeholder that is used to hold types that need to be resolved during the resolving process
    /// </summary>
    internal class InternalToBeResolvedPlaceholder : IInternalToBeResolvedPlaceholder
    {
        public InternalToBeResolvedPlaceholder(Type resolvedType, IRegistration resolvedRegistration, List<object?>? parameters)
        {
            ResolvedType = resolvedType;
            ResolvedRegistration = resolvedRegistration;
            Parameters = parameters;
        }

        /// <summary>
        /// The <see cref="Type"/> to be resolved
        /// </summary>
        public Type ResolvedType { get; }
        
        /// <summary>
        /// The <see cref="IRegistration"/> to be resolved
        /// </summary>
        public IRegistration ResolvedRegistration { get; }
        
        /// <summary>
        /// The parameters needed to resolve the <see cref="ResolvedRegistration"/>
        /// </summary>
        public List<object?>? Parameters { get; }
    }
}