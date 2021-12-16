// Author: Gockner, Simon
// Created: 2021-12-15
// Copyright(c) 2021 SimonG. All Rights Reserved.

using System;

namespace LightweightIocContainer.Interfaces.Registrations;

/// <summary>
/// Creates and collects the <see cref="IRegistration"/>s
/// </summary>
public interface IRegistrationCollector
{
    /// <summary>
    /// Add an Interface with a Type that implements it
    /// </summary>
    /// <typeparam name="TInterface">The Interface to add</typeparam>
    /// <typeparam name="TImplementation">The Type that implements the interface</typeparam>
    /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IRegistration"/></param>
    /// <returns>The created <see cref="IRegistration"/></returns>
    ITypedRegistration<TInterface, TImplementation> Add<TInterface, TImplementation>(Lifestyle lifestyle = Lifestyle.Transient) where TImplementation : TInterface;

    /// <summary>
    /// Add an open generic Interface with an open generic Type that implements it
    /// </summary>
    /// <param name="tInterface">The open generic Interface to add</param>
    /// <param name="tImplementation">The open generic Type that implements the interface</param>
    /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IRegistration"/></param>
    /// <returns>The created <see cref="IRegistration"/></returns>
    IOpenGenericRegistration AddOpenGenerics(Type tInterface, Type tImplementation, Lifestyle lifestyle = Lifestyle.Transient);

    /// <summary>
    /// Add multiple interfaces for a <see cref="Type"/> that implements them
    /// </summary>
    /// <typeparam name="TInterface1">The base interface to add</typeparam>
    /// <typeparam name="TInterface2">A second interface to add</typeparam>
    /// <typeparam name="TImplementation">The <see cref="Type"/> that implements both interfaces</typeparam>
    /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IRegistration"/></param>
    /// <returns>The created <see cref="IMultipleRegistration{TInterface1,TInterface2}"/></returns>
    IMultipleRegistration<TInterface1, TInterface2, TImplementation> Add<TInterface1, TInterface2, TImplementation>(Lifestyle lifestyle = Lifestyle.Transient) where TImplementation : TInterface2, TInterface1;

    /// <summary>
    /// Add multiple interfaces for a <see cref="Type"/> that implements them
    /// </summary>
    /// <typeparam name="TInterface1">The base interface to add</typeparam>
    /// <typeparam name="TInterface2">A second interface to add</typeparam>
    /// <typeparam name="TInterface3">A third interface to add</typeparam>
    /// <typeparam name="TImplementation">The <see cref="Type"/> that implements both interfaces</typeparam>
    /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IRegistration"/></param>
    /// <returns>The created <see cref="IMultipleRegistration{TInterface1,TInterface2,TInterface3}"/></returns>
    IMultipleRegistration<TInterface1, TInterface2, TInterface3, TImplementation> Add<TInterface1, TInterface2, TInterface3, TImplementation>(Lifestyle lifestyle = Lifestyle.Transient) where TImplementation : TInterface3, TInterface2, TInterface1;

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
    IMultipleRegistration<TInterface1, TInterface2, TInterface3, TInterface4, TImplementation> Add<TInterface1, TInterface2, TInterface3, TInterface4, TImplementation>(Lifestyle lifestyle = Lifestyle.Transient) where TImplementation : TInterface4, TInterface3, TInterface2, TInterface1;

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
    IMultipleRegistration<TInterface1, TInterface2, TInterface3, TInterface4, TInterface5, TImplementation> Add<TInterface1, TInterface2, TInterface3, TInterface4, TInterface5, TImplementation>(Lifestyle lifestyle = Lifestyle.Transient) where TImplementation : TInterface5, TInterface4, TInterface3, TInterface2, TInterface1;

    /// <summary>
    /// Add a <see cref="Type"/> without an interface
    /// </summary>
    /// <typeparam name="TImplementation">The <see cref="Type"/> to add</typeparam>
    /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IRegistration"/></param>
    /// <returns>The created <see cref="IRegistration"/></returns>
    ISingleTypeRegistration<TImplementation> Add<TImplementation>(Lifestyle lifestyle = Lifestyle.Transient);

    /// <summary>
    /// Add an Interface with a Type that implements it as a multiton
    /// </summary>
    /// <typeparam name="TInterface">The Interface to add</typeparam>
    /// <typeparam name="TImplementation">The Type that implements the interface</typeparam>
    /// <typeparam name="TScope">The Type of the multiton scope</typeparam>
    /// <returns>The created <see cref="IRegistration"/></returns>
    IMultitonRegistration<TInterface, TImplementation> AddMultiton<TInterface, TImplementation, TScope>() where TImplementation : TInterface;

    /// <summary>
    /// Add multiple interfaces for a <see cref="Type"/> that implements them as a multiton
    /// </summary>
    /// <typeparam name="TInterface1">The base interface to add</typeparam>
    /// <typeparam name="TInterface2">A second interface to add</typeparam>
    /// <typeparam name="TImplementation">The Type that implements the interface</typeparam>
    /// <typeparam name="TScope">The Type of the multiton scope</typeparam>
    /// <returns>The created <see cref="IRegistration"/></returns>
    IMultipleMultitonRegistration<TInterface1, TInterface2, TImplementation> AddMultiton<TInterface1, TInterface2, TImplementation, TScope>() where TImplementation : TInterface1, TInterface2;
}