// // Author: Simon Gockner
// // Created: 2020-01-29
// // Copyright(c) 2020 SimonG. All Rights Reserved.

namespace LightweightIocContainer.Interfaces.Registrations;

/// <summary>
/// Provides a <see cref="LightweightIocContainer.Lifestyle"/> to an <see cref="IRegistration"/>
/// </summary>
public interface ILifestyleProvider
{
    /// <summary>
    /// The Lifestyle of Instances that are created with this <see cref="IRegistration"/>
    /// </summary>
    Lifestyle Lifestyle { get; }
}