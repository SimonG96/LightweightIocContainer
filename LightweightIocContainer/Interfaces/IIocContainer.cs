// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using LightweightIocContainer.Exceptions;
using LightweightIocContainer.Interfaces.Installers;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Interfaces;

/// <summary>
/// The main container that carries all <see cref="IRegistration"/>s
/// </summary>
public interface IIocContainer : IDisposable
{
    /// <summary>
    /// Install the given installers for the current <see cref="IIocContainer"/>
    /// </summary>
    /// <param name="installers">The given <see cref="IIocInstaller"/>s</param>
    /// <returns>An instance of the current <see cref="IIocContainer"/></returns>
    IIocContainer Install(params IIocInstaller[] installers);

    /// <summary>
    /// Register an <see cref="IRegistration"/> at this <see cref="IocContainer"/> 
    /// </summary>
    /// <param name="addRegistration">The <see cref="Func{T, TResult}"/> that creates an <see cref="IRegistration"/></param>
    public void Register(Func<IRegistrationCollector, IRegistration> addRegistration);

    /// <summary>
    /// Clear the multiton instances of the given <see cref="Type"/> from the registered multitons list
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> to clear the multiton instances</typeparam>
    void ClearMultitonInstances<T>();

    /// <summary>
    /// Is the given <see cref="Type"/> registered with this <see cref="IIocContainer"/>
    /// </summary>
    /// <typeparam name="T">The given <see cref="Type"/></typeparam>
    /// <returns>True if the given <see cref="Type"/> is registered with this <see cref="IIocContainer"/>, false if not</returns>
    bool IsTypeRegistered<T>();

    /// <summary>
    /// Register a custom <see cref="Attribute"/> that can annotate a constructor to be ignored
    /// </summary>
    /// <typeparam name="T">The custom <see cref="Attribute"/></typeparam>
    /// <exception cref="InvalidIgnoreConstructorAttributeException{T}">The passed <see cref="Attribute"/> can't be used on a constructor</exception>
    void RegisterIgnoreConstructorAttribute<T>() where T : Attribute;
}