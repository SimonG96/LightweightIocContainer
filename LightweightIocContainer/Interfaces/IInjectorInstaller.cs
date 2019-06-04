// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Interfaces
{
    /// <summary>
    /// The base class for <see cref="IInjectorContainer"/> installers
    /// </summary>
    public interface IInjectorInstaller
    {
        /// <summary>
        /// Install the needed <see cref="IRegistrationBase"/>s in the given <see cref="IInjectorContainer"/>
        /// </summary>
        /// <param name="container">The current <see cref="IInjectorContainer"/></param>
        void Install(IInjectorContainer container);
    }
}