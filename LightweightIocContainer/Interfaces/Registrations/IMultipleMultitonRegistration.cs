// Author: Gockner, Simon
// Created: 2020-11-19
// Copyright(c) 2020 SimonG. All Rights Reserved.

namespace LightweightIocContainer.Interfaces.Registrations
{
    public interface IMultipleMultitonRegistration<TInterface1, TInterface2, TImplementation> : IMultitonRegistration<TInterface1, TImplementation>, IMultipleRegistration<TInterface1, TInterface2, TImplementation> where TImplementation : TInterface1, TInterface2
    {
        
    }
}