// Author: Simon Gockner
// Created: 2019-12-07
// Copyright(c) 2019 SimonG. All Rights Reserved.

using LightweightIocContainer.Interfaces;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Registrations;

/// <summary>
/// The base class for every <see cref="IMultipleRegistration{TInterface1,TInterface2, TImplementation}"/> to register multiple interfaces
/// </summary>
/// <typeparam name="TInterface1">The first interface</typeparam>
/// <typeparam name="TImplementation">The implementation</typeparam>
internal abstract class MultipleRegistration<TInterface1, TImplementation> : TypedRegistration<TInterface1, TImplementation>, IMultipleRegistration<TInterface1, TImplementation> where TImplementation : TInterface1
{
    /// <summary>
    /// The base class for every <see cref="IMultipleRegistration{TInterface1,TInterface2}"/> to register multiple interfaces
    /// </summary>
    /// <param name="interfaceType1">The <see cref="Type"/> of the first interface</param>
    /// <param name="implementationType">The <see cref="Type"/> of the implementation</param>
    /// <param name="lifestyle">The <see cref="Lifestyle"/> of this <see cref="MultipleRegistration{TInterface1,TInterface2}"/></param>
    /// <param name="container">The current instance of the <see cref="IIocContainer"/></param>
    protected MultipleRegistration(Type interfaceType1, Type implementationType, Lifestyle lifestyle, IocContainer container)
        : base(interfaceType1, implementationType, lifestyle, container) =>
        Registrations = [];

    /// <summary>
    /// A <see cref="List{T}"/> of <see cref="IRegistration"/>s that are registered within this <see cref="MultipleRegistration{TInterface1,TInterface2}"/>
    /// </summary>
    public List<IRegistration> Registrations { get; protected init; }
}

/// <summary>
/// An <see cref="IRegistration"/> to register multiple interfaces for on implementation type
/// </summary>
/// <typeparam name="TInterface1">The first interface</typeparam>
/// <typeparam name="TInterface2">The second interface</typeparam>
/// <typeparam name="TImplementation">The implementation</typeparam>
internal class MultipleRegistration<TInterface1, TInterface2, TImplementation> : MultipleRegistration<TInterface1, TImplementation>, IMultipleRegistration<TInterface1, TInterface2, TImplementation> where TImplementation : TInterface1, TInterface2
{
    /// <summary>
    /// An <see cref="IRegistration"/> to register multiple interfaces for on implementation type
    /// </summary>
    /// <param name="interfaceType1">The <see cref="Type"/> of the first interface</param>
    /// <param name="interfaceType2">The <see cref="Type"/> of the second interface</param>
    /// <param name="implementationType">The <see cref="Type"/> of the implementation</param>
    /// <param name="lifestyle">The <see cref="Lifestyle"/> of this <see cref="MultipleRegistration{TInterface1,TInterface2}"/></param>
    /// <param name="container">The current instance of the <see cref="IIocContainer"/></param>
    public MultipleRegistration(Type interfaceType1, Type interfaceType2, Type implementationType, Lifestyle lifestyle, IocContainer container)
        : base(interfaceType1, implementationType, lifestyle, container)
    {
        Registrations =
        [
            new TypedRegistration<TInterface1, TImplementation>(interfaceType1, implementationType, lifestyle, container),
            new TypedRegistration<TInterface2, TImplementation>(interfaceType2, implementationType, lifestyle, container)
        ];
    }

    /// <summary>
    /// Pass an <see cref="Action{T}"/> that will be invoked when an instance of this type is created
    /// </summary>
    /// <param name="action">The <see cref="Action{T}"/></param>
    /// <returns>The current instance of this <see cref="ITypedRegistration{TInterface,TImplementation}"/></returns>
    public override ITypedRegistration<TInterface1, TImplementation> OnCreate(Action<TImplementation?> action)
    {
        foreach (IRegistration registration in Registrations)
        {
            if (registration is ITypedRegistration<TInterface2, TImplementation> interface2Registration)
                interface2Registration.OnCreate(action);
            else if (registration is ITypedRegistration<TInterface1, TImplementation> interface1Registration)
                interface1Registration.OnCreate(action);
        }

        return this;
    }
}

/// <summary>
/// An <see cref="IRegistration"/> to register multiple interfaces for on implementation type
/// </summary>
/// <typeparam name="TInterface1">The first interface</typeparam>
/// <typeparam name="TInterface2">The second interface</typeparam>
/// <typeparam name="TInterface3">The third interface</typeparam>
/// <typeparam name="TImplementation">The implementation</typeparam>
internal class MultipleRegistration<TInterface1, TInterface2, TInterface3, TImplementation> : MultipleRegistration<TInterface1, TImplementation>, IMultipleRegistration<TInterface1, TInterface2, TInterface3, TImplementation> where TImplementation : TInterface3, TInterface2, TInterface1
{
    /// <summary>
    /// An <see cref="IRegistration"/> to register multiple interfaces for on implementation type
    /// </summary>
    /// <param name="interfaceType1">The <see cref="Type"/> of the first interface</param>
    /// <param name="interfaceType2">The <see cref="Type"/> of the second interface</param>
    /// <param name="interfaceType3">The <see cref="Type"/> of the third interface</param>
    /// <param name="implementationType">The <see cref="Type"/> of the implementation</param>
    /// <param name="lifestyle">The <see cref="Lifestyle"/> of this <see cref="MultipleRegistration{TInterface1,TInterface2}"/></param>
    /// <param name="container">The current instance of the <see cref="IIocContainer"/></param>
    public MultipleRegistration(Type interfaceType1, Type interfaceType2, Type interfaceType3, Type implementationType, Lifestyle lifestyle, IocContainer container)
        : base(interfaceType1, implementationType, lifestyle, container)
    {
        Registrations =
        [
            new TypedRegistration<TInterface1, TImplementation>(interfaceType1, implementationType, lifestyle, container),
            new TypedRegistration<TInterface2, TImplementation>(interfaceType2, implementationType, lifestyle, container),
            new TypedRegistration<TInterface3, TImplementation>(interfaceType3, implementationType, lifestyle, container)
        ];
    }

    /// <summary>
    /// Pass an <see cref="Action{T}"/> that will be invoked when an instance of this type is created
    /// </summary>
    /// <param name="action">The <see cref="Action{T}"/></param>
    /// <returns>The current instance of this <see cref="ITypedRegistration{TInterface,TImplementation}"/></returns>
    public override ITypedRegistration<TInterface1, TImplementation> OnCreate(Action<TImplementation?> action)
    {
        foreach (IRegistration registration in Registrations)
        {
            if (registration is ITypedRegistration<TInterface3, TImplementation> interface3Registration)
                interface3Registration.OnCreate(action);
            else if (registration is ITypedRegistration<TInterface2, TImplementation> interface2Registration)
                interface2Registration.OnCreate(action);
            else if (registration is ITypedRegistration<TInterface1, TImplementation> interface1Registration)
                interface1Registration.OnCreate(action);
        }

        return this;
    }
}

/// <summary>
/// An <see cref="IRegistration"/> to register multiple interfaces for on implementation type
/// </summary>
/// <typeparam name="TInterface1">The first interface</typeparam>
/// <typeparam name="TInterface2">The second interface</typeparam>
/// <typeparam name="TInterface3">The third interface</typeparam>
/// <typeparam name="TInterface4">The fourth interface</typeparam>
/// <typeparam name="TImplementation">The implementation</typeparam>
internal class MultipleRegistration<TInterface1, TInterface2, TInterface3, TInterface4, TImplementation> : MultipleRegistration<TInterface1, TImplementation>, IMultipleRegistration<TInterface1, TInterface2, TInterface3, TInterface4, TImplementation> where TImplementation : TInterface4, TInterface3, TInterface2, TInterface1
{
    /// <summary>
    /// An <see cref="IRegistration"/> to register multiple interfaces for on implementation type
    /// </summary>
    /// <param name="interfaceType1">The <see cref="Type"/> of the first interface</param>
    /// <param name="interfaceType2">The <see cref="Type"/> of the second interface</param>
    /// <param name="interfaceType3">The <see cref="Type"/> of the third interface</param>
    /// <param name="interfaceType4">The <see cref="Type"/> of the fourth interface</param>
    /// <param name="implementationType">The <see cref="Type"/> of the implementation</param>
    /// <param name="lifestyle">The <see cref="Lifestyle"/> of this <see cref="MultipleRegistration{TInterface1,TInterface2}"/></param>
    /// <param name="container">The current instance of the <see cref="IIocContainer"/></param>
    public MultipleRegistration(Type interfaceType1, Type interfaceType2, Type interfaceType3, Type interfaceType4, Type implementationType, Lifestyle lifestyle, IocContainer container)
        : base(interfaceType1, implementationType, lifestyle, container)
    {
        Registrations =
        [
            new TypedRegistration<TInterface1, TImplementation>(interfaceType1, implementationType, lifestyle, container),
            new TypedRegistration<TInterface2, TImplementation>(interfaceType2, implementationType, lifestyle, container),
            new TypedRegistration<TInterface3, TImplementation>(interfaceType3, implementationType, lifestyle, container),
            new TypedRegistration<TInterface4, TImplementation>(interfaceType4, implementationType, lifestyle, container)
        ];
    }

    /// <summary>
    /// Pass an <see cref="Action{T}"/> that will be invoked when an instance of this type is created
    /// </summary>
    /// <param name="action">The <see cref="Action{T}"/></param>
    /// <returns>The current instance of this <see cref="ITypedRegistration{TInterface,TImplementation}"/></returns>
    public override ITypedRegistration<TInterface1, TImplementation> OnCreate(Action<TImplementation?> action)
    {
        foreach (IRegistration registration in Registrations)
        {
            if (registration is ITypedRegistration<TInterface4, TImplementation> interface4Registration)
                interface4Registration.OnCreate(action);
            else if (registration is ITypedRegistration<TInterface3, TImplementation> interface3Registration)
                interface3Registration.OnCreate(action);
            else if (registration is ITypedRegistration<TInterface2, TImplementation> interface2Registration)
                interface2Registration.OnCreate(action);
            else if (registration is ITypedRegistration<TInterface1, TImplementation> interface1Registration)
                interface1Registration.OnCreate(action);
        }

        return this;
    }
}

/// <summary>
/// An <see cref="IRegistration"/> to register multiple interfaces for on implementation type
/// </summary>
/// <typeparam name="TInterface1">The first interface</typeparam>
/// <typeparam name="TInterface2">The second interface</typeparam>
/// <typeparam name="TInterface3">The third interface</typeparam>
/// <typeparam name="TInterface4">The fourth interface</typeparam>
/// <typeparam name="TInterface5">The fifth interface</typeparam>
/// <typeparam name="TImplementation">The implementation</typeparam>
internal class MultipleRegistration<TInterface1, TInterface2, TInterface3, TInterface4, TInterface5, TImplementation> : MultipleRegistration<TInterface1, TImplementation>, IMultipleRegistration<TInterface1, TInterface2, TInterface3, TInterface4, TInterface5, TImplementation> where TImplementation : TInterface5, TInterface4, TInterface3, TInterface2, TInterface1
{
    /// <summary>
    /// An <see cref="IRegistration"/> to register multiple interfaces for on implementation type
    /// </summary>
    /// <param name="interfaceType1">The <see cref="Type"/> of the first interface</param>
    /// <param name="interfaceType2">The <see cref="Type"/> of the second interface</param>
    /// <param name="interfaceType3">The <see cref="Type"/> of the third interface</param>
    /// <param name="interfaceType4">The <see cref="Type"/> of the fourth interface</param>
    /// <param name="interfaceType5">The <see cref="Type"/> of the fifth interface</param>
    /// <param name="implementationType">The <see cref="Type"/> of the implementation</param>
    /// <param name="lifestyle">The <see cref="Lifestyle"/> of this <see cref="MultipleRegistration{TInterface1,TInterface2}"/></param>
    /// <param name="container">The current instance of the <see cref="IIocContainer"/></param>
    public MultipleRegistration(Type interfaceType1, Type interfaceType2, Type interfaceType3, Type interfaceType4, Type interfaceType5, Type implementationType, Lifestyle lifestyle, IocContainer container)
        : base(interfaceType1, implementationType, lifestyle, container)
    {
        Registrations =
        [
            new TypedRegistration<TInterface1, TImplementation>(interfaceType1, implementationType, lifestyle, container),
            new TypedRegistration<TInterface2, TImplementation>(interfaceType2, implementationType, lifestyle, container),
            new TypedRegistration<TInterface3, TImplementation>(interfaceType3, implementationType, lifestyle, container),
            new TypedRegistration<TInterface4, TImplementation>(interfaceType4, implementationType, lifestyle, container),
            new TypedRegistration<TInterface5, TImplementation>(interfaceType5, implementationType, lifestyle, container)
        ];
    }

    /// <summary>
    /// Pass an <see cref="Action{T}"/> that will be invoked when an instance of this type is created
    /// </summary>
    /// <param name="action">The <see cref="Action{T}"/></param>
    /// <returns>The current instance of this <see cref="ITypedRegistration{TInterface,TImplementation}"/></returns>
    public override ITypedRegistration<TInterface1, TImplementation> OnCreate(Action<TImplementation?> action)
    {
        foreach (IRegistration registration in Registrations)
        {
            if (registration is ITypedRegistration<TInterface5, TImplementation> interface5Registration)
                interface5Registration.OnCreate(action);
            else if (registration is ITypedRegistration<TInterface4, TImplementation> interface4Registration)
                interface4Registration.OnCreate(action);
            else if (registration is ITypedRegistration<TInterface3, TImplementation> interface3Registration)
                interface3Registration.OnCreate(action);
            else if (registration is ITypedRegistration<TInterface2, TImplementation> interface2Registration)
                interface2Registration.OnCreate(action);
            else if (registration is ITypedRegistration<TInterface1, TImplementation> interface1Registration)
                interface1Registration.OnCreate(action);
        }

        return this;
    }
}