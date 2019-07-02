// // Author: Gockner, Simon
// // Created: 2019-06-07
// // Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Registrations
{
    /// <summary>
    /// The registration that is used to register a multiton
    /// </summary>
    /// <typeparam name="TInterface">The registered interface</typeparam>
    public class MultitonRegistration<TInterface> : DefaultRegistration<TInterface>, IMultitonRegistration<TInterface>
    {
        /// <summary>
        /// The registration that is used to register a multiton
        /// </summary>
        /// <param name="interfaceType">The <see cref="Type"/> of the Interface</param>
        /// <param name="implementationType">The <see cref="Type"/> of the Implementation</param>
        /// <param name="scope">The <see cref="Type"/> of the Multiton Scope</param>
        public MultitonRegistration(Type interfaceType, Type implementationType, Type scope)
            : base(interfaceType, implementationType, Lifestyle.Multiton)
        {
            Scope = scope;
        }

        /// <summary>
        /// The <see cref="Type"/> of the multiton scope
        /// </summary>
        public Type Scope { get; }
    }
}