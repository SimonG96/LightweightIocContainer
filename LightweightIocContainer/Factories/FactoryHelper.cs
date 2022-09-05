// Author: Gockner, Simon
// Created: 2022-09-02
// Copyright(c) 2022 SimonG. All Rights Reserved.

using System.Reflection;

namespace LightweightIocContainer.Factories;

/// <summary>
/// Helper class for the <see cref="TypedFactory{TFactory}"/>
/// </summary>
public class FactoryHelper
{
    /// <summary>
    /// Convert `null` passed as argument to <see cref="NullParameter"/> to handle it correctly
    /// </summary>
    /// <param name="createMethod">The create method of the factory</param>
    /// <param name="arguments">The arguments passed to the create method</param>
    /// <returns>List of arguments with converted null</returns>
    /// <exception cref="Exception">Wrong parameters passed</exception>
    public object?[] ConvertPassedNull(MethodBase createMethod, params object?[] arguments)
    {
        if (!arguments.Any())
            return arguments;

        ParameterInfo[] parameters = createMethod.GetParameters();
        if (arguments.Length != parameters.Length)
            throw new Exception("Wrong parameters passed");
            
        for (int i = 0; i < arguments.Length; i++) 
            arguments[i] ??= new NullParameter(parameters[i].ParameterType);

        return arguments;
    }
}