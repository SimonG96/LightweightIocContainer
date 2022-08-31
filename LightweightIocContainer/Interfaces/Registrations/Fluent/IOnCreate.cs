// Author: Simon Gockner
// Created: 2019-12-07
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Interfaces.Installers;

namespace LightweightIocContainer.Interfaces.Registrations.Fluent;

/// <summary>
/// Provides an <see cref="OnCreateAction"/> to the generic <see cref="IOnCreate{TInterface, TImplementation}"/>
/// </summary>
public interface IOnCreate
{
    /// <summary>
    /// This <see cref="Action"/> is invoked when an instance of this type is created.
    /// <para>Can be set in the <see cref="IIocInstaller"/> by calling <see cref="IOnCreate{TInterface, TImplementation}.OnCreate"/></para>
    /// </summary>
    internal Action<object?>? OnCreateAction { get; }
}

/// <summary>
/// Provides an <see cref="OnCreate"/> method to an <see cref="IRegistrationBase"/>
/// </summary>
/// <typeparam name="TInterface">The registered interface</typeparam>
/// <typeparam name="TImplementation">The registered implementation</typeparam>
public interface IOnCreate<TInterface, TImplementation> : IOnCreate where TImplementation : TInterface
{
    /// <summary>
    /// Pass an <see cref="Action{T}"/> that will be invoked when an instance of this type is created
    /// </summary>
    /// <param name="action">The <see cref="Action{T}"/></param>
    /// <returns>The current instance of this <see cref="ITypedRegistration{TInterface,TImplementation}"/></returns>
    ITypedRegistration<TInterface, TImplementation> OnCreate(Action<TImplementation?> action);
}