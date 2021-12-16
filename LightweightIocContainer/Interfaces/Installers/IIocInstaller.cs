// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Interfaces.Installers
{
    /// <summary>
    /// The base class for <see cref="IIocContainer"/> installers
    /// </summary>
    public interface IIocInstaller
    {
        /// <summary>
        /// Install the needed <see cref="IRegistration"/>s in the given <see cref="IIocContainer"/>
        /// </summary>
        /// <param name="registration">The <see cref="IRegistrationCollector"/> where <see cref="IRegistration"/>s are added</param>
        void Install(IRegistrationCollector registration);
    }
}