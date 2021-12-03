// Author: Gockner, Simon
// Created: 2021-12-02
// Copyright(c) 2021 SimonG. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using LightweightIocContainer.Interfaces.Registrations.Fluent;

namespace LightweightIocContainer.Validation
{
    /// <summary>
    /// Validator for your <see cref="IocContainer"/> to check if everything can be resolved with your current setup
    /// </summary>
    public class IocValidator
    {
        private readonly IocContainer _iocContainer;
        private readonly List<(Type type, object parameter)> _parameters;

        /// <summary>
        /// Validator for your <see cref="IocContainer"/> to check if everything can be resolved with your current setup
        /// </summary>
        /// <param name="iocContainer">The <see cref="IocContainer"/></param>
        public IocValidator(IocContainer iocContainer)
        {
            _iocContainer = iocContainer;
            _parameters = new List<(Type, object)>();
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
        /// </summary>
        public void Validate()
        {
            foreach (var registration in _iocContainer.Registrations)
            {
                if (registration is IWithFactory { Factory: { } } withFactoryRegistration)
                {
                    (from createMethod in withFactoryRegistration.Factory.CreateMethods
                        select createMethod.GetParameters().Select(p => p.ParameterType)
                        into parameterTypes 
                        let definedParameters = _parameters.Where(p => p.type == registration.InterfaceType)
                        select (from parameterType in parameterTypes
                            let definedParameter = definedParameters
                                .FirstOrDefault(p => parameterType.IsInstanceOfType(p.parameter))
                            select definedParameter == default ? parameterType.GetDefault() : definedParameter.parameter).ToArray())
                        .ToList()
                        .ForEach(p => _iocContainer.Resolve(registration.InterfaceType, p, null));
                }
                else
                    _iocContainer.Resolve(registration.InterfaceType, null, null);
            }
        }
    }
}