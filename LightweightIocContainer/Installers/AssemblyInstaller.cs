// // Author: Gockner, Simon
// // Created: 2019-06-12
// // Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Reflection;
using LightweightIocContainer.Interfaces;
using LightweightIocContainer.Interfaces.Installers;

namespace LightweightIocContainer.Installers
{
    public class AssemblyInstaller : IAssemblyInstaller
    {
        public AssemblyInstaller(Assembly assembly)
        {
            Installers = new List<IIocInstaller>();

            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                if (!typeof(IIocInstaller).IsAssignableFrom(type))
                    continue;

                Installers.Add((IIocInstaller) Activator.CreateInstance(type));
            }
        }

        public List<IIocInstaller> Installers { get; }

        public void Install(IIocContainer container)
        {
            foreach (var installer in Installers)
            {
                installer.Install(container);
            }
        }
    }
}