﻿// Author: Gockner, Simon
// Created: 2021-12-02
// Copyright(c) 2021 SimonG. All Rights Reserved.

using LightweightIocContainer.Interfaces.Registrations;
using LightweightIocContainer.Interfaces.Registrations.Fluent;
using NSubstitute;

namespace LightweightIocContainer.Validation;

/// <summary>
/// Validator for your <see cref="IocContainer"/> to check if everything can be resolved with your current setup
/// </summary>
public class IocValidator(IocContainer iocContainer)
{
    private readonly List<(Type type, object? parameter)> _parameters = [];

    /// <summary>
    /// Add parameters that can't be default for your type to be created successfully
    /// </summary>
    /// <param name="parameter">The new value of the parameter</param>
    /// <typeparam name="TInterface">The <see cref="Type"/> of your registered interface</typeparam>
    /// <typeparam name="TParameter">The <see cref="Type"/> of your <paramref name="parameter"></paramref></typeparam>
    public void AddParameter<TInterface, TParameter>(TParameter parameter) => _parameters.Add((typeof(TInterface), parameter));
        
    /// <summary>
    /// Validates your given <see cref="IocContainer"/> and checks if everything can be resolved with the current setup
    /// <exception cref="AggregateException">Collection of all exceptions that are thrown during validation</exception>
    /// </summary>
    public void Validate()
    {
        List<Exception> validationExceptions = [];
            
        foreach (IRegistration registration in iocContainer.Registrations)
        {
            var definedParameters = _parameters.Where(p => p.type == registration.InterfaceType);
                
            if (registration is IWithFactoryInternal { Factory: not null } withFactoryRegistration)
            {
                (from createMethod in withFactoryRegistration.Factory.CreateMethods.Where(m => m.ReturnType == registration.InterfaceType)
                        select createMethod.GetParameters().Select(p => p.ParameterType)
                        into parameterTypes 
                        select (from parameterType in parameterTypes
                            let definedParameter = definedParameters
                                .FirstOrDefault(p => parameterType.IsInstanceOfType(p.parameter))
                            select definedParameter == default ? GetMockOrDefault(parameterType) : definedParameter.parameter).ToArray())
                    .ToList()
                    .ForEach(p => TryResolve(registration.InterfaceType, p, validationExceptions, true));
            }
            else
            {
                object?[] arguments = definedParameters.Select(p => p.parameter).ToArray();
                TryResolve(registration.InterfaceType, arguments, validationExceptions);
            }
        }

        if (validationExceptions.Any())
            throw new AggregateException("Validation failed.", validationExceptions);
    }

    private void TryResolve(Type type, object?[]? arguments, List<Exception> validationExceptions, bool isFactoryResolve = false)
    {
        if (type.ContainsGenericParameters)
        {
            List<Type> genericParameters = [];
            
            Type[] genericArguments = type.GetGenericArguments();
            foreach (Type genericArgument in genericArguments.Where(g => g.IsGenericParameter))
            {
                Type[] genericParameterConstraints = genericArgument.GetGenericParameterConstraints();
                if (genericParameterConstraints.Any())
                {
                    object mock = Substitute.For(genericParameterConstraints, []);
                    genericParameters.Add(mock.GetType());
                }
                else
                    genericParameters.Add(typeof(object));
            }

            type = type.MakeGenericType(genericParameters.ToArray());
        }
        
        (bool success, object _, Exception? exception) = iocContainer.TryResolveNonGeneric(type, arguments, null, isFactoryResolve);
        if (success)
            return;
        
        if (exception is not null)
            validationExceptions.Add(exception);
    }

    private T GetMock<T>() where T : class => Substitute.For<T>();
    private object? GetMockOrDefault(Type type)
    {
        if (type.IsValueType)
            return Activator.CreateInstance(type);

        if (type == typeof(string))
            return string.Empty;

        if (!type.IsInterface)
            return new NullParameter(type);
            
        try
        {
            return GenericMethodCaller.CallPrivate(this, nameof(GetMock), type);
        }
        catch (Exception)
        {
            return new NullParameter(type); 
        }
    }
}