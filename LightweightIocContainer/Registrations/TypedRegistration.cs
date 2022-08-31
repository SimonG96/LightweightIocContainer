﻿// Author: Simon Gockner
// Created: 2019-12-14
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Interfaces;
using LightweightIocContainer.Interfaces.Installers;
using LightweightIocContainer.Interfaces.Registrations;
using LightweightIocContainer.Interfaces.Registrations.Fluent;

namespace LightweightIocContainer.Registrations;

/// <summary>
/// A <see cref="IRegistrationBase"/> that implements a <see cref="Type"/>
/// </summary>
internal class TypedRegistration<TInterface, TImplementation> : RegistrationBase, ITypedRegistration<TInterface, TImplementation> where TImplementation : TInterface
{
    /// <summary>
    /// A <see cref="IRegistrationBase"/> that implements a <see cref="Type"/>
    /// </summary>
    /// <param name="interfaceType">The <see cref="Type"/> of the interface</param>
    /// <param name="implementationType">The <see cref="Type"/> of the implementation type</param>
    /// <param name="lifestyle">The <see cref="Lifestyle"/> of this <see cref="IRegistrationBase"/></param>
    /// <param name="container">The current instance of the <see cref="IIocContainer"/></param>
    public TypedRegistration(Type interfaceType, Type implementationType, Lifestyle lifestyle, IocContainer container)
        : base(interfaceType, lifestyle, container) =>
        ImplementationType = implementationType;

    /// <summary>
    /// The <see cref="Type"/> that implements the <see cref="IRegistration.InterfaceType"/> that is registered with this <see cref="IRegistrationBase"/>
    /// </summary>
    public Type ImplementationType { get; }

    /// <summary>
    /// This <see cref="Action"/> is invoked when an instance of this type is created.
    /// <para>Can be set in the <see cref="IIocInstaller"/> by calling <see cref="IOnCreate{TInterface,TImplementation}.OnCreate"/></para>
    /// </summary>
    private Action<object?>? OnCreateAction { get; set; }

    /// <summary>
    /// This <see cref="Action"/> is invoked when an instance of this type is created.
    /// <para>Can be set in the <see cref="IIocInstaller"/> by calling <see cref="IOnCreate{TInterface,TImplementation}.OnCreate"/></para>
    /// </summary>
    Action<object?>? IOnCreate.OnCreateAction => OnCreateAction;

    /// <summary>
    /// Pass an <see cref="Action{T}"/> that will be invoked when an instance of this type is created
    /// </summary>
    /// <param name="action">The <see cref="Action{T}"/></param>
    /// <returns>The current instance of this <see cref="ITypedRegistration{TInterface,TImplementation}"/></returns>
    public virtual ITypedRegistration<TInterface, TImplementation> OnCreate(Action<TImplementation?> action)
    {
        OnCreateAction = a => action((TImplementation?) a);
        return this;
    }

    /// <summary>
    /// Validate the <see cref="DisposeStrategy"/> for the <see cref="ImplementationType"/> and <see cref="Lifestyle"/>
    /// </summary>
    protected override void ValidateDisposeStrategy() => ValidateDisposeStrategy(ImplementationType);

    public override bool Equals(object? obj)
    {
        if (obj is not TypedRegistration<TInterface, TImplementation> typedRegistration)
            return false;

        if (!base.Equals(obj))
            return false;

        if (OnCreateAction == null && typedRegistration.OnCreateAction != null)
            return false;

        if (OnCreateAction != null && typedRegistration.OnCreateAction == null)
            return false;
            
        return ImplementationType == typedRegistration.ImplementationType;
    }

    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), ImplementationType);
}