// Author: Gockner, Simon
// Created: 2019-10-15
// Copyright(c) 2019 SimonG. All Rights Reserved.

namespace LightweightIocContainer.Interfaces.Registrations
{
    /// <summary>
    /// A special <see cref="IDefaultRegistration{TInterface}"/> that allows to set an <see cref="ResolveCallback{T}"/> as a callback that is called on <see cref="IIocContainer.Resolve{T}()"/>
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    public interface IUnitTestCallbackRegistration<out TInterface> : IRegistrationBase
    {
        /// <summary>
        /// An <see cref="ResolveCallback{T}"/> that is set as a callback that is called on <see cref="IIocContainer.Resolve{T}()"/>
        /// </summary>
        ResolveCallback<TInterface> UnitTestResolveCallback { get; }
    }
}