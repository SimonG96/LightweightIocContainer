// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using LightweightIocContainer.Interfaces.Factories;

namespace LightweightIocContainer.Factories
{
    /// <summary>
    /// Class to help implement an abstract typed factory
    /// </summary>
    /// <typeparam name="TFactory">The type of the abstract factory</typeparam>
    public class TypedFactory<TFactory> : ITypedFactory<TFactory>
    {
        /// <summary>
        /// The implemented abstract typed factory/>
        /// </summary>
        public TFactory Factory { get; set; }
    }
}