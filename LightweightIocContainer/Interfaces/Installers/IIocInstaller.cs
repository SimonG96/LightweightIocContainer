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
        /// <param name="container">The current <see cref="IIocContainer"/></param>
        void Install(IIocContainer container);
    }
}