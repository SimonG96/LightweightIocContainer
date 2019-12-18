// Author: Simon Gockner
// Created: 2019-12-07
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using System.Collections.Generic;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Registrations
{
    /// <summary>
    /// The base class for every <see cref="IMultipleRegistration{TInterface1,TInterface2}"/> to register multiple interfaces
    /// </summary>
    /// <typeparam name="TInterface1">The first interface</typeparam>
    public abstract class MultipleRegistration<TInterface1> : TypedRegistrationBase<TInterface1>, IMultipleRegistration<TInterface1>
    {
        /// <summary>
        /// The base class for every <see cref="IMultipleRegistration{TInterface1,TInterface2}"/> to register multiple interfaces
        /// </summary>
        /// <param name="interfaceType1">The <see cref="Type"/> of the first interface</param>
        /// <param name="implementationType">The <see cref="Type"/> of the implementation</param>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> of this <see cref="MultipleRegistration{TInterface1,TInterface2}"/></param>
        protected MultipleRegistration(Type interfaceType1, Type implementationType, Lifestyle lifestyle)
            : base(interfaceType1, implementationType, lifestyle)
        {
            
        }

        /// <summary>
        /// A <see cref="List{T}"/> of <see cref="IRegistration"/>s that are registered within this <see cref="MultipleRegistration{TInterface1,TInterface2}"/>
        /// </summary>
        public List<IRegistration> Registrations { get; protected set; }
    }

    /// <summary>
    /// An <see cref="IRegistrationBase{TInterface}"/> to register multiple interfaces for on implementation type
    /// </summary>
    /// <typeparam name="TInterface1">The first interface</typeparam>
    /// <typeparam name="TInterface2">The second interface</typeparam>
    public class MultipleRegistration<TInterface1, TInterface2> : MultipleRegistration<TInterface1>, IMultipleRegistration<TInterface1, TInterface2>
    {
        /// <summary>
        /// An <see cref="IRegistrationBase{TInterface}"/> to register multiple interfaces for on implementation type
        /// </summary>
        /// <param name="interfaceType1">The <see cref="Type"/> of the first interface</param>
        /// <param name="interfaceType2">The <see cref="Type"/> of the second interface</param>
        /// <param name="implementationType">The <see cref="Type"/> of the implementation</param>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> of this <see cref="MultipleRegistration{TInterface1,TInterface2}"/></param>
        public MultipleRegistration(Type interfaceType1, Type interfaceType2, Type implementationType, Lifestyle lifestyle)
            : base(interfaceType1, implementationType, lifestyle)
        {
            Registrations = new List<IRegistration>() 
            { 
                new DefaultRegistration<TInterface1>(interfaceType1, implementationType, lifestyle),
                new DefaultRegistration<TInterface2>(interfaceType2, implementationType, lifestyle)
            };
        }

        /// <summary>
        /// Pass an <see cref="Action{T}"/> for each interface that will be invoked when instances of the types are created
        /// </summary>
        /// <param name="action1">The first <see cref="Action{T}"/></param>
        /// <param name="action2">The second <see cref="Action{T}"/></param>
        /// <returns>The current instance of this <see cref="MultipleRegistration{TInterface1,TInterface2}"/></returns>
        public IMultipleRegistration<TInterface1, TInterface2> OnCreate(Action<TInterface1> action1, Action<TInterface2> action2)
        {
            foreach (var registration in Registrations)
            {
                if (registration is IDefaultRegistration<TInterface2> interface2Registration)
                    interface2Registration.OnCreate(action2);
                else if (registration is IDefaultRegistration<TInterface1> interface1Registration)
                    interface1Registration.OnCreate(action1);
            }

            return this;
        }
    }

    /// <summary>
    /// An <see cref="IRegistrationBase{TInterface}"/> to register multiple interfaces for on implementation type
    /// </summary>
    /// <typeparam name="TInterface1">The first interface</typeparam>
    /// <typeparam name="TInterface2">The second interface</typeparam>
    /// <typeparam name="TInterface3">The third interface</typeparam>
    public class MultipleRegistration<TInterface1, TInterface2, TInterface3> : MultipleRegistration<TInterface1>, IMultipleRegistration<TInterface1, TInterface2, TInterface3>
    {
        /// <summary>
        /// An <see cref="IRegistrationBase{TInterface}"/> to register multiple interfaces for on implementation type
        /// </summary>
        /// <param name="interfaceType1">The <see cref="Type"/> of the first interface</param>
        /// <param name="interfaceType2">The <see cref="Type"/> of the second interface</param>
        /// <param name="interfaceType3">The <see cref="Type"/> of the third interface</param>
        /// <param name="implementationType">The <see cref="Type"/> of the implementation</param>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> of this <see cref="MultipleRegistration{TInterface1,TInterface2}"/></param>
        public MultipleRegistration(Type interfaceType1, Type interfaceType2, Type interfaceType3, Type implementationType, Lifestyle lifestyle)
            : base(interfaceType1, implementationType, lifestyle)
        {
            Registrations = new List<IRegistration>()
            {
                new DefaultRegistration<TInterface1>(interfaceType1, implementationType, lifestyle),
                new DefaultRegistration<TInterface2>(interfaceType2, implementationType, lifestyle),
                new DefaultRegistration<TInterface3>(interfaceType3, implementationType, lifestyle)
            };
        }

        /// <summary>
        /// Pass an <see cref="Action{T}"/> for each interface that will be invoked when instances of the types are created
        /// </summary>
        /// <param name="action1">The first <see cref="Action{T}"/></param>
        /// <param name="action2">The second <see cref="Action{T}"/></param>
        /// <param name="action3">The third <see cref="Action{T}"/></param>
        /// <returns>The current instance of this <see cref="MultipleRegistration{TInterface1,TInterface2}"/></returns>
        public IMultipleRegistration<TInterface1, TInterface2, TInterface3> OnCreate(Action<TInterface1> action1, Action<TInterface2> action2, Action<TInterface3> action3)
        {
            foreach (var registration in Registrations)
            {
                if (registration is IDefaultRegistration<TInterface3> interface3Registration)
                    interface3Registration.OnCreate(action3);
                else if (registration is IDefaultRegistration<TInterface2> interface2Registration)
                    interface2Registration.OnCreate(action2);
                else if (registration is IDefaultRegistration<TInterface1> interface1Registration)
                    interface1Registration.OnCreate(action1);
            }

            return this;
        }
    }

    /// <summary>
    /// An <see cref="IRegistrationBase{TInterface}"/> to register multiple interfaces for on implementation type
    /// </summary>
    /// <typeparam name="TInterface1">The first interface</typeparam>
    /// <typeparam name="TInterface2">The second interface</typeparam>
    /// <typeparam name="TInterface3">The third interface</typeparam>
    /// <typeparam name="TInterface4">The fourth interface</typeparam>
    public class MultipleRegistration<TInterface1, TInterface2, TInterface3, TInterface4> : MultipleRegistration<TInterface1>, IMultipleRegistration<TInterface1, TInterface2, TInterface3, TInterface4>
    {
        /// <summary>
        /// An <see cref="IRegistrationBase{TInterface}"/> to register multiple interfaces for on implementation type
        /// </summary>
        /// <param name="interfaceType1">The <see cref="Type"/> of the first interface</param>
        /// <param name="interfaceType2">The <see cref="Type"/> of the second interface</param>
        /// <param name="interfaceType3">The <see cref="Type"/> of the third interface</param>
        /// <param name="interfaceType4">The <see cref="Type"/> of the fourth interface</param>
        /// <param name="implementationType">The <see cref="Type"/> of the implementation</param>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> of this <see cref="MultipleRegistration{TInterface1,TInterface2}"/></param>
        public MultipleRegistration(Type interfaceType1, Type interfaceType2, Type interfaceType3, Type interfaceType4, Type implementationType, Lifestyle lifestyle)
            : base(interfaceType1, implementationType, lifestyle)
        {
            Registrations = new List<IRegistration>()
            {
                new DefaultRegistration<TInterface1>(interfaceType1, implementationType, lifestyle),
                new DefaultRegistration<TInterface2>(interfaceType2, implementationType, lifestyle),
                new DefaultRegistration<TInterface3>(interfaceType3, implementationType, lifestyle),
                new DefaultRegistration<TInterface4>(interfaceType4, implementationType, lifestyle)
            };
        }

        /// <summary>
        /// Pass an <see cref="Action{T}"/> for each interface that will be invoked when instances of the types are created
        /// </summary>
        /// <param name="action1">The first <see cref="Action{T}"/></param>
        /// <param name="action2">The second <see cref="Action{T}"/></param>
        /// <param name="action3">The third <see cref="Action{T}"/></param>
        /// <param name="action4">The fourth <see cref="Action{T}"/></param>
        /// <returns>The current instance of this <see cref="MultipleRegistration{TInterface1,TInterface2}"/></returns>
        public IMultipleRegistration<TInterface1, TInterface2, TInterface3, TInterface4> OnCreate(Action<TInterface1> action1, Action<TInterface2> action2, Action<TInterface3> action3, Action<TInterface4> action4)
        {
            foreach (var registration in Registrations)
            {
                if (registration is IDefaultRegistration<TInterface4> interface4Registration)
                    interface4Registration.OnCreate(action4);
                else if (registration is IDefaultRegistration<TInterface3> interface3Registration)
                    interface3Registration.OnCreate(action3);
                else if (registration is IDefaultRegistration<TInterface2> interface2Registration)
                    interface2Registration.OnCreate(action2);
                else if (registration is IDefaultRegistration<TInterface1> interface1Registration)
                    interface1Registration.OnCreate(action1);
            }

            return this;
        }
    }

    /// <summary>
    /// An <see cref="IRegistrationBase{TInterface}"/> to register multiple interfaces for on implementation type
    /// </summary>
    /// <typeparam name="TInterface1">The first interface</typeparam>
    /// <typeparam name="TInterface2">The second interface</typeparam>
    /// <typeparam name="TInterface3">The third interface</typeparam>
    /// <typeparam name="TInterface4">The fourth interface</typeparam>
    /// <typeparam name="TInterface5">The fifth interface</typeparam>
    public class MultipleRegistration<TInterface1, TInterface2, TInterface3, TInterface4, TInterface5> : MultipleRegistration<TInterface1>, IMultipleRegistration<TInterface1, TInterface2, TInterface3, TInterface4, TInterface5>
    {
        /// <summary>
        /// An <see cref="IRegistrationBase{TInterface}"/> to register multiple interfaces for on implementation type
        /// </summary>
        /// <param name="interfaceType1">The <see cref="Type"/> of the first interface</param>
        /// <param name="interfaceType2">The <see cref="Type"/> of the second interface</param>
        /// <param name="interfaceType3">The <see cref="Type"/> of the third interface</param>
        /// <param name="interfaceType4">The <see cref="Type"/> of the fourth interface</param>
        /// <param name="interfaceType5">The <see cref="Type"/> of the fifth interface</param>
        /// <param name="implementationType">The <see cref="Type"/> of the implementation</param>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> of this <see cref="MultipleRegistration{TInterface1,TInterface2}"/></param>
        public MultipleRegistration(Type interfaceType1, Type interfaceType2, Type interfaceType3, Type interfaceType4, Type interfaceType5, Type implementationType, Lifestyle lifestyle)
            : base(interfaceType1, implementationType, lifestyle)
        {
            Registrations = new List<IRegistration>()
            {
                new DefaultRegistration<TInterface1>(interfaceType1, implementationType, lifestyle),
                new DefaultRegistration<TInterface2>(interfaceType2, implementationType, lifestyle),
                new DefaultRegistration<TInterface3>(interfaceType3, implementationType, lifestyle),
                new DefaultRegistration<TInterface4>(interfaceType4, implementationType, lifestyle),
                new DefaultRegistration<TInterface5>(interfaceType5, implementationType, lifestyle)
            };
        }

        /// <summary>
        /// Pass an <see cref="Action{T}"/> for each interface that will be invoked when instances of the types are created
        /// </summary>
        /// <param name="action1">The first <see cref="Action{T}"/></param>
        /// <param name="action2">The second <see cref="Action{T}"/></param>
        /// <param name="action3">The third <see cref="Action{T}"/></param>
        /// <param name="action4">The fourth <see cref="Action{T}"/></param>
        /// <param name="action5">The fifth <see cref="Action{T}"/></param>
        /// <returns>The current instance of this <see cref="MultipleRegistration{TInterface1,TInterface2}"/></returns>
        public IMultipleRegistration<TInterface1, TInterface2, TInterface3, TInterface4, TInterface5> OnCreate(Action<TInterface1> action1, Action<TInterface2> action2, Action<TInterface3> action3, Action<TInterface4> action4, Action<TInterface5> action5)
        {
            foreach (var registration in Registrations)
            {
                if (registration is IDefaultRegistration<TInterface5> interface5Registration)
                    interface5Registration.OnCreate(action5);
                else if (registration is IDefaultRegistration<TInterface4> interface4Registration)
                    interface4Registration.OnCreate(action4);
                else if (registration is IDefaultRegistration<TInterface3> interface3Registration)
                    interface3Registration.OnCreate(action3);
                else if (registration is IDefaultRegistration<TInterface2> interface2Registration)
                    interface2Registration.OnCreate(action2);
                else if (registration is IDefaultRegistration<TInterface1> interface1Registration)
                    interface1Registration.OnCreate(action1);
            }

            return this;
        }
    }
}