// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer
{
    /// <summary>
    /// The Lifestyles that can be used for a <see cref="IDefaultRegistration{TInterface}"/>
    /// </summary>
    public enum Lifestyle
    {
        /// <summary>
        /// A new instance gets created every time an instance is resolved
        /// </summary>
        Transient,

        /// <summary>
        /// One instance is created that gets returned every time an instance is resolved
        /// </summary>
        Singleton

        //TODO: Add Lifestyle.Multiton
    }
}