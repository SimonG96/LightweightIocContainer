// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

namespace LightweightIocContainer.Interfaces.Factories
{
    /// <summary>
    /// Class to help implement an abstract typed factory
    /// </summary>
    /// <typeparam name="TFactory">The type of the abstract factory</typeparam>
    public interface ITypedFactory<TFactory>
    {
        /// <summary>
        /// The implemented abstract typed factory
        /// </summary>
        TFactory Factory { get; set; }
    }
}