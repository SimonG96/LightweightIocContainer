// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System.Collections.Generic;
using System.Reflection;

namespace LightweightIocContainer.Interfaces.Factories;

/// <summary>
/// Non-generic <see cref="ITypedFactory{TFactory}"/>
/// </summary>
public interface ITypedFactory
{
    /// <summary>
    /// The create methods of this <see cref="ITypedFactory"/>
    /// </summary>
    List<MethodInfo> CreateMethods { get; }
}
    
/// <summary>
/// Class to help implement an abstract typed factory
/// </summary>
/// <typeparam name="TFactory">The type of the abstract factory</typeparam>
public interface ITypedFactory<TFactory> : ITypedFactory
{
    /// <summary>
    /// The implemented abstract typed factory
    /// </summary>
    TFactory Factory { get; set; }
}