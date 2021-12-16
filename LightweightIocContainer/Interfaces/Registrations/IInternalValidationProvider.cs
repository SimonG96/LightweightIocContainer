// Author: Gockner, Simon
// Created: 2021-12-15
// Copyright(c) 2021 SimonG. All Rights Reserved.

namespace LightweightIocContainer.Interfaces.Registrations;

/// <summary>
/// Provides an internal <see cref="Validate"/> method to an <see cref="IRegistrationBase"/>
/// </summary>
internal interface IInternalValidationProvider
{
    /// <summary>
    /// Validate this <see cref="IRegistrationBase"/>
    /// </summary>
    void Validate();
}