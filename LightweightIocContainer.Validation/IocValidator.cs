// Author: Gockner, Simon
// Created: 2021-12-02
// Copyright(c) 2021 SimonG. All Rights Reserved.

using LightweightIocContainer.Interfaces.Registrations;
using LightweightIocContainer.Interfaces.Registrations.Fluent;
using Moq;

namespace LightweightIocContainer.Validation;

/// <summary>
/// Validator for your <see cref="IocContainer"/> to check if everything can be resolved with your current setup
/// </summary>
public class IocValidator
{
    private readonly IocContainer _iocContainer;
    private readonly List<(Type type, object? parameter)> _parameters;

    /// <summary>
    /// Validator for your <see cref="IocContainer"/> to check if everything can be resolved with your current setup
    /// </summary>
    /// <param name="iocContainer">The <see cref="IocContainer"/></param>
    public IocValidator(IocContainer iocContainer)
    {
        _iocContainer = iocContainer;
        _parameters = new List<(Type, object?)>();
    }

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
        List<Exception> validationExceptions = new();
            
        foreach (IRegistration registration in _iocContainer.Registrations)
        {
            var definedParameters = _parameters.Where(p => p.type == registration.InterfaceType);
                
            if (registration is IWithFactoryInternal { Factory: { } } withFactoryRegistration)
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
        try
        {
            _iocContainer.TryResolveNonGeneric(type, arguments, null, isFactoryResolve);
        }
        catch (Exception exception)
        {
            validationExceptions.Add(exception);
        }
    }

    private T GetMock<T>() where T : class => new Mock<T>().Object;
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