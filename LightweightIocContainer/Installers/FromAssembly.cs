// // Author: Gockner, Simon
// // Created: 2019-06-12
// // Copyright(c) 2019 SimonG. All Rights Reserved.

using System.Reflection;
using LightweightIocContainer.Interfaces.Installers;

namespace LightweightIocContainer.Installers
{
    public static class FromAssembly
    {
        public static IAssemblyInstaller This()
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            return new AssemblyInstaller(assembly);
        }

        public static IAssemblyInstaller Instance(Assembly assembly)
        {
            return new AssemblyInstaller(assembly);
        }
    }
}