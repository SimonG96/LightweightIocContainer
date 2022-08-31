// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System.Reflection;
using System.Reflection.Emit;
using LightweightIocContainer.Exceptions;
using LightweightIocContainer.Interfaces;
using LightweightIocContainer.Interfaces.Factories;

namespace LightweightIocContainer.Factories;

/// <summary>
/// Class to help implement an abstract typed factory
/// </summary>
/// <typeparam name="TFactory">The type of the abstract factory</typeparam>
public class TypedFactory<TFactory> : TypedFactoryBase<TFactory>, ITypedFactory<TFactory>
{
    private const string CLEAR_MULTITON_INSTANCE_METHOD_NAME = "ClearMultitonInstance";

    /// <summary>
    /// The 
    /// </summary>
    /// <param name="container">The current instance of the <see cref="IIocContainer"/></param>
    public TypedFactory(IocContainer container) => Factory = CreateFactory(container);

    /// <summary>
    /// The implemented abstract typed factory/>
    /// </summary>
    public TFactory Factory { get; set; }
        
    /// <summary>
    /// Creates the factory from the given abstract factory type
    /// </summary>
    /// <exception cref="InvalidFactoryRegistrationException">Factory registration is invalid</exception>
    /// <exception cref="IllegalAbstractMethodCreationException">Creation of abstract methods are illegal in their current state</exception>
    private TFactory CreateFactory(IocContainer container)
    {
        Type factoryType = typeof(TFactory);
            
        AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("Factory"), AssemblyBuilderAccess.Run);
        ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("Factory");
        TypeBuilder typeBuilder = moduleBuilder.DefineType($"TypedFactory.{factoryType.Name}");
            
        typeBuilder.AddInterfaceImplementation(factoryType);

        //add `private readonly IIocContainer _container` field
        FieldBuilder containerFieldBuilder = typeBuilder.DefineField("_container", typeof(IocContainer), FieldAttributes.Private | FieldAttributes.InitOnly);

        //add ctor
        ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.HasThis, new[] {typeof(IocContainer)});
        ILGenerator constructorGenerator = constructorBuilder.GetILGenerator();
        constructorGenerator.Emit(OpCodes.Ldarg_0);
        constructorGenerator.Emit(OpCodes.Ldarg_1);
        constructorGenerator.Emit(OpCodes.Stfld, containerFieldBuilder); //set `_container` field
        constructorGenerator.Emit(OpCodes.Ret);

        foreach (MethodInfo createMethod in CreateMethods)
        {
            //create a method that looks like this
            //public `createMethod.ReturnType` Create(`createMethod.GetParameters()`)
            //{
            //    return IIocContainer.Resolve(`createMethod.ReturnType`, params);
            //}

            ParameterInfo[] args = createMethod.GetParameters();

            MethodBuilder methodBuilder = typeBuilder.DefineMethod(createMethod.Name, MethodAttributes.Public | MethodAttributes.Virtual, 
                createMethod.ReturnType, (from arg in args select arg.ParameterType).ToArray());
            typeBuilder.DefineMethodOverride(methodBuilder, createMethod);

            ILGenerator generator = methodBuilder.GetILGenerator();

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, containerFieldBuilder);

            if (args.Any())
            {
                generator.Emit(OpCodes.Ldc_I4_S, args.Length);
                generator.Emit(OpCodes.Newarr, typeof(object));

                for (int i = 0; i < args.Length; i++)
                {
                    generator.Emit(OpCodes.Dup);
                    generator.Emit(OpCodes.Ldc_I4_S, i);
                    generator.Emit(OpCodes.Ldarg_S, i + 1);
                    generator.Emit(OpCodes.Box, args[i].ParameterType); //Boxing is only needed for simple datatypes, but for now it is not a problem to box everything
                    generator.Emit(OpCodes.Stelem_Ref);
                }
            }
            else
            {
                MethodInfo emptyArray = typeof(Array).GetMethod(nameof(Array.Empty))!.MakeGenericMethod(typeof(object));
                generator.EmitCall(OpCodes.Call, emptyArray, null);
            }

            generator.EmitCall(OpCodes.Call, typeof(IocContainer).GetMethod(nameof(IocContainer.FactoryResolve), new[] { typeof(object[]) })!.MakeGenericMethod(createMethod.ReturnType), null);
            generator.Emit(OpCodes.Castclass, createMethod.ReturnType);
            generator.Emit(OpCodes.Ret);
        }

        //if factory contains a method to clear multiton instances
        MethodInfo? multitonClearMethod = factoryType.GetMethods().FirstOrDefault(m => m.Name.Equals(CLEAR_MULTITON_INSTANCE_METHOD_NAME));
        if (multitonClearMethod != null)
        {
            //create a method that looks like this
            //public void ClearMultitonInstance<typeToClear>()
            //{
            //    IIocContainer.ClearMultitonInstances<typeToClear>();
            //}

            if (multitonClearMethod.IsGenericMethod)
            {
                Type? typeToClear = multitonClearMethod.GetGenericArguments().FirstOrDefault();
                if (typeToClear == null)
                    throw new IllegalAbstractMethodCreationException("No Type to clear specified.", multitonClearMethod);

                MethodBuilder multitonClearMethodBuilder = typeBuilder.DefineMethod(multitonClearMethod.Name, MethodAttributes.Public | MethodAttributes.Virtual,
                    multitonClearMethod.ReturnType, null);
                multitonClearMethodBuilder.DefineGenericParameters(typeToClear.Name);

                typeBuilder.DefineMethodOverride(multitonClearMethodBuilder, multitonClearMethod);

                ILGenerator multitonClearGenerator = multitonClearMethodBuilder.GetILGenerator();
                multitonClearGenerator.Emit(OpCodes.Ldarg_0);
                multitonClearGenerator.Emit(OpCodes.Ldfld, containerFieldBuilder);

                multitonClearGenerator.EmitCall(OpCodes.Call, typeof(IocContainer).GetMethod(nameof(IocContainer.ClearMultitonInstances))!.MakeGenericMethod(typeToClear), null);
                multitonClearGenerator.Emit(OpCodes.Ret);
            }
            else
            {
                throw new IllegalAbstractMethodCreationException("No Type to clear specified.", multitonClearMethod);
            }
        }

        return Creator.CreateInstance<TFactory>(typeBuilder.CreateTypeInfo()!.AsType(), container);
    }
}