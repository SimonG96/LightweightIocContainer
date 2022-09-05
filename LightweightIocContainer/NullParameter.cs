// Author: Gockner, Simon
// Created: 2022-09-02
// Copyright(c) 2022 SimonG. All Rights Reserved.

namespace LightweightIocContainer;

/// <summary>
/// Wrapper class to handle null passed as an argument correctly
/// </summary>
public class NullParameter
{
    /// <summary>
    /// Wrapper class to handle null passed as an argument correctly
    /// </summary>
    /// <param name="parameterType">The <see cref="Type"/> of the parameter</param>
    public NullParameter(Type parameterType) => ParameterType = parameterType;
    
    /// <summary>
    /// The <see cref="Type"/> of the parameter
    /// </summary>
    public Type ParameterType { get; }
}