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
    /// <summary>
    /// An <see cref="IIocInstaller"/> that installs all <see cref="IIocInstaller"/>s for its given <see cref="Assembly"/>
    /// </summary>
    public class AssemblyInstaller : IAssemblyInstaller
    {
        /// <summary>
        /// An <see cref="IIocInstaller"/> that installs all <see cref="IIocInstaller"/>s for its given <see cref="Assembly"/>
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> from where the <see cref="IIocInstaller"/>s will be installed</param>
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

        /// <summary>
        /// The <see cref="IIocInstaller"/>s of the Assembly that this <see cref="AssemblyInstaller"/> is installing
        /// </summary>
        public List<IIocInstaller> Installers { get; }

        /// <summary>
        /// Install the found <see cref="IIocInstaller"/>s in the given <see cref="IIocContainer"/>
        /// </summary>
        /// <param name="container">The current <see cref="IIocContainer"/></param>
        public void Install(IIocContainer container)
        {
            foreach (IIocInstaller installer in Installers)
            {
                installer.Install(container);
            }
        }
    }
}