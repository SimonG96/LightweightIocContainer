// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using LightweightIocContainer.Exceptions;
using LightweightIocContainer.Factories;
using LightweightIocContainer.Interfaces;
using LightweightIocContainer.Interfaces.Factories;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Registrations
{
    /// <summary>
    /// The registration that is used to register an abstract typed factory
    /// </summary>
    /// <typeparam name="TFactory">The type of the abstract typed factory</typeparam>
    public class TypedFactoryRegistration<TFactory> : ITypedFactoryRegistration<TFactory>
    {
        private readonly IIocContainer _container;

        public TypedFactoryRegistration(Type factoryType, IIocContainer container)
        {
            _container = container;

            InterfaceType = factoryType;
            Name = $"{InterfaceType.Name}";

            CreateFactory();
        }

        /// <summary>
        /// The name of the <see cref="TypedFactoryRegistration{TFactory}"/>
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The Type of the abstract typed factory that is registered with this <see cref="TypedFactoryRegistration{TFactory}"/>
        /// </summary>
        public Type InterfaceType { get; }

        /// <summary>
        /// The class that contains the implemented abstract factory of this <see cref="TypedFactoryRegistration{TFactory}"/>
        /// </summary>
        public ITypedFactory<TFactory> Factory { get; private set; }


        /// <summary>
        /// Creates the factory from the given abstract factory type
        /// </summary>
        /// <exception cref="InvalidFactoryRegistrationException">Factory registration is invalid</exception>
        private void CreateFactory()
        {
            var createMethods = InterfaceType.GetMethods().Where(m => m.ReturnType != typeof(void)).ToList();
            if (!createMethods.Any())
                throw new InvalidFactoryRegistrationException($"Factory {Name} has no create methods.");

            Type type = typeof(TypedFactory<>);
            Type factory = type.MakeGenericType(InterfaceType);
            
            Factory = (ITypedFactory<TFactory>) Activator.CreateInstance(factory);
            
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("Factory"), AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("Factory");
            TypeBuilder typeBuilder = moduleBuilder.DefineType($"TypedFactory.{InterfaceType.Name}");
            
            typeBuilder.AddInterfaceImplementation(InterfaceType);

            //add `private readonly IIocContainer _container` field
            FieldBuilder containerFieldBuilder = typeBuilder.DefineField("_container", typeof(IIocContainer), FieldAttributes.Private | FieldAttributes.InitOnly);

            //add ctor
            ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.HasThis, new[] {typeof(IIocContainer)});
            var constructorGenerator = constructorBuilder.GetILGenerator();
            constructorGenerator.Emit(OpCodes.Ldarg_0);
            constructorGenerator.Emit(OpCodes.Ldarg_1);
            constructorGenerator.Emit(OpCodes.Stfld, containerFieldBuilder); //set `_container` field
            constructorGenerator.Emit(OpCodes.Ret);

            foreach (var createMethod in createMethods)
            {
                //create a method that looks like this
                //public `createMethod.ReturnType` Create(`createMethod.GetParameters()`)
                //{
                //    return IIocContainer.Resolve(`createMethod.ReturnType`, params);
                //}

                var args = createMethod.GetParameters();

                MethodBuilder methodBuilder = typeBuilder.DefineMethod(createMethod.Name, MethodAttributes.Public | MethodAttributes.Virtual, 
                    createMethod.ReturnType, (from arg in args select arg.ParameterType).ToArray());
                typeBuilder.DefineMethodOverride(methodBuilder, createMethod);

                var generator = methodBuilder.GetILGenerator();

                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldfld, containerFieldBuilder);
                generator.Emit(OpCodes.Ldtoken, createMethod.ReturnType);

                MethodInfo getTypeFromHandle = typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle));
                generator.EmitCall(OpCodes.Call, getTypeFromHandle, null);

                if (args.Any())
                {
                    generator.Emit(OpCodes.Ldc_I4_S, args.Length);
                    generator.Emit(OpCodes.Newarr, typeof(object));

                    for (int i = 0; i < args.Length; i++)
                    {
                        generator.Emit(OpCodes.Dup);
                        generator.Emit(OpCodes.Ldc_I4_S, i);
                        generator.Emit(OpCodes.Ldarg_S, i + 1);
                        generator.Emit(OpCodes.Stelem_Ref);
                    }
                }
                else
                {
                    MethodInfo emptyArray = typeof(Array).GetMethod(nameof(Array.Empty))?.MakeGenericMethod(typeof(object));
                    generator.EmitCall(OpCodes.Call, emptyArray, null);
                }

                generator.EmitCall(OpCodes.Callvirt, typeof(IIocContainer).GetMethod(nameof(IIocContainer.Resolve), new[] { typeof(Type), typeof(object[])}), null);
                generator.Emit(OpCodes.Castclass, createMethod.ReturnType);
                generator.Emit(OpCodes.Ret);
            }

            Factory.Factory = (TFactory) Activator.CreateInstance(typeBuilder.CreateTypeInfo().AsType(), _container);
        }
    }
}