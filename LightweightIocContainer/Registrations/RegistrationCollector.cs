// Author: Gockner, Simon
// Created: 2021-12-15
// Copyright(c) 2021 SimonG. All Rights Reserved.

using System;
using System.Collections.Generic;
using LightweightIocContainer.Exceptions;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Registrations;

/// <summary>
/// Creates and collects the <see cref="IRegistration"/>s
/// </summary>
public class RegistrationCollector : IRegistrationCollector
{
    private readonly RegistrationFactory _registrationFactory;

    internal RegistrationCollector(RegistrationFactory registrationFactory)
    {
        _registrationFactory = registrationFactory;
        Registrations = new List<IRegistration>();
    }

    /// <summary>
    /// The collected <see cref="IRegistration"/>s
    /// </summary>
    internal List<IRegistration> Registrations { get; }

    /// <summary>
    /// Add an Interface with a Type that implements it
    /// </summary>
    /// <typeparam name="TInterface">The Interface to add</typeparam>
    /// <typeparam name="TImplementation">The Type that implements the interface</typeparam>
    /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IRegistration"/></param>
    /// <returns>The created <see cref="IRegistration"/></returns>
    public ITypedRegistration<TInterface, TImplementation> Add<TInterface, TImplementation>(Lifestyle lifestyle = Lifestyle.Transient) where TImplementation : TInterface
    {
        ITypedRegistration<TInterface, TImplementation> registration = _registrationFactory.Register<TInterface, TImplementation>(lifestyle);
        Registrations.Add(registration);

        return registration;
    }

    /// <summary>
    /// Add an open generic Interface with an open generic Type that implements it
    /// </summary>
    /// <param name="tInterface">The open generic Interface to add</param>
    /// <param name="tImplementation">The open generic Type that implements the interface</param>
    /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IRegistration"/></param>
    /// <returns>The created <see cref="IRegistration"/></returns>
    /// <exception cref="InvalidRegistrationException">Function can only be used to register open generic types</exception>
    /// <exception cref="InvalidRegistrationException">Can't register a multiton with open generic registration</exception>
    public IOpenGenericRegistration AddOpenGenerics(Type tInterface, Type tImplementation, Lifestyle lifestyle = Lifestyle.Transient)
    {
        IOpenGenericRegistration registration = _registrationFactory.Register(tInterface, tImplementation, lifestyle);
        Registrations.Add(registration);

        return registration;
    }

    /// <summary>
    /// Add multiple interfaces for a <see cref="Type"/> that implements them
    /// </summary>
    /// <typeparam name="TInterface1">The base interface to add</typeparam>
    /// <typeparam name="TInterface2">A second interface to add</typeparam>
    /// <typeparam name="TImplementation">The <see cref="Type"/> that implements both interfaces</typeparam>
    /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IRegistration"/></param>
    /// <returns>The created <see cref="IMultipleRegistration{TInterface1,TInterface2}"/></returns>
    public IMultipleRegistration<TInterface1, TInterface2, TImplementation> Add<TInterface1, TInterface2, TImplementation>(Lifestyle lifestyle = Lifestyle.Transient) where TImplementation : TInterface2, TInterface1
    {
        IMultipleRegistration<TInterface1, TInterface2, TImplementation> multipleRegistration = _registrationFactory.Register<TInterface1, TInterface2, TImplementation>(lifestyle);
        Register(multipleRegistration);

        return multipleRegistration;
    }

    /// <summary>
    /// Add multiple interfaces for a <see cref="Type"/> that implements them
    /// </summary>
    /// <typeparam name="TInterface1">The base interface to add</typeparam>
    /// <typeparam name="TInterface2">A second interface to add</typeparam>
    /// <typeparam name="TInterface3">A third interface to add</typeparam>
    /// <typeparam name="TImplementation">The <see cref="Type"/> that implements both interfaces</typeparam>
    /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IRegistration"/></param>
    /// <returns>The created <see cref="IMultipleRegistration{TInterface1,TInterface2,TInterface3}"/></returns>
    public IMultipleRegistration<TInterface1, TInterface2, TInterface3, TImplementation> Add<TInterface1, TInterface2, TInterface3, TImplementation>(Lifestyle lifestyle = Lifestyle.Transient) where TImplementation : TInterface3, TInterface2, TInterface1
    {
        IMultipleRegistration<TInterface1, TInterface2, TInterface3, TImplementation> multipleRegistration = _registrationFactory.Register<TInterface1, TInterface2, TInterface3, TImplementation>(lifestyle);
        Register(multipleRegistration);

        return multipleRegistration;
    }

    /// <summary>
    /// Add multiple interfaces for a <see cref="Type"/> that implements them
    /// </summary>
    /// <typeparam name="TInterface1">The base interface to add</typeparam>
    /// <typeparam name="TInterface2">A second interface to add</typeparam>
    /// <typeparam name="TInterface3">A third interface to add</typeparam>
    /// <typeparam name="TInterface4">A fourth interface to add</typeparam>
    /// <typeparam name="TImplementation">The <see cref="Type"/> that implements both interfaces</typeparam>
    /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IRegistration"/></param>
    /// <returns>The created <see cref="IMultipleRegistration{TInterface1,TInterface2,TInterface3,TInterface4}"/></returns>
    public IMultipleRegistration<TInterface1, TInterface2, TInterface3, TInterface4, TImplementation> Add<TInterface1, TInterface2, TInterface3, TInterface4, TImplementation>(Lifestyle lifestyle = Lifestyle.Transient) where TImplementation : TInterface4, TInterface3, TInterface2, TInterface1
    {
        IMultipleRegistration<TInterface1, TInterface2, TInterface3, TInterface4, TImplementation> multipleRegistration = _registrationFactory.Register<TInterface1, TInterface2, TInterface3, TInterface4, TImplementation>(lifestyle);
        Register(multipleRegistration);

        return multipleRegistration;
    }

    /// <summary>
    /// Add multiple interfaces for a <see cref="Type"/> that implements them
    /// </summary>
    /// <typeparam name="TInterface1">The base interface to add</typeparam>
    /// <typeparam name="TInterface2">A second interface to add</typeparam>
    /// <typeparam name="TInterface3">A third interface to add</typeparam>
    /// <typeparam name="TInterface4">A fourth interface to add</typeparam>
    /// <typeparam name="TInterface5">A fifth interface to add</typeparam>
    /// <typeparam name="TImplementation">The <see cref="Type"/> that implements both interfaces</typeparam>
    /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IRegistration"/></param>
    /// <returns>The created <see cref="IMultipleRegistration{TInterface1,TInterface2,TInterface3,TInterface4,TInterface5}"/></returns>
    public IMultipleRegistration<TInterface1, TInterface2, TInterface3, TInterface4, TInterface5, TImplementation> Add<TInterface1, TInterface2, TInterface3, TInterface4, TInterface5, TImplementation>(Lifestyle lifestyle = Lifestyle.Transient) where TImplementation : TInterface5, TInterface4, TInterface3, TInterface2, TInterface1
    {
        IMultipleRegistration<TInterface1, TInterface2, TInterface3, TInterface4, TInterface5, TImplementation> multipleRegistration = _registrationFactory.Register<TInterface1, TInterface2, TInterface3, TInterface4, TInterface5, TImplementation>(lifestyle);
        Register(multipleRegistration);

        return multipleRegistration;
    }

    /// <summary>
    /// Add a <see cref="Type"/> without an interface
    /// </summary>
    /// <typeparam name="TImplementation">The <see cref="Type"/> to add</typeparam>
    /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IRegistration"/></param>
    /// <returns>The created <see cref="IRegistration"/></returns>
    public ISingleTypeRegistration<TImplementation> Add<TImplementation>(Lifestyle lifestyle = Lifestyle.Transient)
    {
        ISingleTypeRegistration<TImplementation> registration = _registrationFactory.Register<TImplementation>(lifestyle);
        Registrations.Add(registration);

        return registration;
    }

    /// <summary>
    /// Add an Interface with a Type that implements it as a multiton
    /// </summary>
    /// <typeparam name="TInterface">The Interface to add</typeparam>
    /// <typeparam name="TImplementation">The Type that implements the interface</typeparam>
    /// <typeparam name="TScope">The Type of the multiton scope</typeparam>
    /// <returns>The created <see cref="IRegistration"/></returns>
    public IMultitonRegistration<TInterface, TImplementation> AddMultiton<TInterface, TImplementation, TScope>() where TImplementation : TInterface
    {
        IMultitonRegistration<TInterface, TImplementation> registration = _registrationFactory.RegisterMultiton<TInterface, TImplementation, TScope>();
        Registrations.Add(registration);

        return registration;
    }

    /// <summary>
    /// Add multiple interfaces for a <see cref="Type"/> that implements them as a multiton
    /// </summary>
    /// <typeparam name="TInterface1">The base interface to add</typeparam>
    /// <typeparam name="TInterface2">A second interface to add</typeparam>
    /// <typeparam name="TImplementation">The Type that implements the interface</typeparam>
    /// <typeparam name="TScope">The Type of the multiton scope</typeparam>
    /// <returns>The created <see cref="IRegistration"/></returns>
    public IMultipleMultitonRegistration<TInterface1, TInterface2, TImplementation> AddMultiton<TInterface1, TInterface2, TImplementation, TScope>() where TImplementation : TInterface1, TInterface2
    {
        IMultipleMultitonRegistration<TInterface1, TInterface2, TImplementation> registration = _registrationFactory.RegisterMultiton<TInterface1, TInterface2, TImplementation, TScope>();
        Register(registration);

        return registration;
    }
    
    /// <summary>
    /// Register all <see cref="IMultipleRegistration{TInterface1,TImplementation}.Registrations"/> from an <see cref="IMultipleRegistration{TInterface1,TImplementation}"/>
    /// </summary>
    /// <typeparam name="TInterface1">The <see cref="Type"/> of the first registered interface</typeparam>
    /// <typeparam name="TImplementation">The <see cref="Type"/> of the registered implementation</typeparam>
    /// <param name="multipleRegistration">The <see cref="IMultipleRegistration{TInterface1,TImplementation}"/></param>
    private void Register<TInterface1, TImplementation>(IMultipleRegistration<TInterface1, TImplementation> multipleRegistration) where TImplementation : TInterface1
    {
        foreach (IRegistration registration in multipleRegistration.Registrations)
            Registrations.Add(registration);
    }
}