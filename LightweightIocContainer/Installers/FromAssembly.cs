// Author: Gockner, Simon
// Created: 2019-06-12
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System.Reflection;
using LightweightIocContainer.Interfaces.Installers;

namespace LightweightIocContainer.Installers
{
    /// <summary>
    /// Helper class that supplies methods to find the wanted <see cref="Assembly"/>
    /// </summary>
    public static class FromAssembly
    {
        /// <summary>
        /// Get an <see cref="IAssemblyInstaller"/> that installs from the <see cref="Assembly"/> calling the method
        /// </summary>
        /// <returns>A new <see cref="IAssemblyInstaller"/> with the calling <see cref="Assembly"/></returns>
        public static IAssemblyInstaller This()
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            return new AssemblyInstaller(assembly);
        }

        /// <summary>
        /// Get an <see cref="IAssemblyInstaller"/> that installs from the given <see cref="Assembly"/>
        /// </summary>
        /// <param name="assembly">The given <see cref="Assembly"/></param>
        /// <returns>A new <see cref="IAssemblyInstaller"/> with the given <see cref="Assembly"/></returns>
        public static IAssemblyInstaller Instance(Assembly assembly) => new AssemblyInstaller(assembly);
    }
}