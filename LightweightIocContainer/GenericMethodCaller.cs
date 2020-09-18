// Author: Simon Gockner
// Created: 2020-09-18
// Copyright(c) 2020 SimonG. All Rights Reserved.

using System;
using System.Reflection;
using LightweightIocContainer.Exceptions;

namespace LightweightIocContainer
{
    public static class GenericMethodCaller
    {
        public static object Call(object caller, string functionName, Type genericParameter, BindingFlags bindingFlags, params object[] parameters)
        {
            MethodInfo method = caller.GetType().GetMethod(functionName, bindingFlags);
            MethodInfo genericMethod = method?.MakeGenericMethod(genericParameter);

            if (genericMethod == null)
                throw new GenericMethodNotFoundException(functionName);

            try //exceptions thrown by methods called with invoke are wrapped into another exception, the exception thrown by the invoked method can be returned by `Exception.GetBaseException()`
            {
                return genericMethod.Invoke(caller, parameters);
            }
            catch (Exception ex)
            {
                throw ex.GetBaseException();
            }
        }
    }
}