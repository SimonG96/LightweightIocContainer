// Author: Gockner, Simon
// Created: 2022-09-01
// Copyright(c) 2022 SimonG. All Rights Reserved.

using LightweightIocContainer.Interfaces;

namespace LightweightIocContainer.Annotations;

/// <summary>
/// If a constructor is annotated with this attribute it will be ignored by the <see cref="IIocContainer"/>
/// </summary>
[AttributeUsage(AttributeTargets.Constructor)]
public class IocIgnoreConstructorAttribute : Attribute
{
    
}