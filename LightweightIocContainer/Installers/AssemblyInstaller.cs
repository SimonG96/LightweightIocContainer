// Author: Gockner, Simon
// Created: 2019-06-12
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System.Reflection;
using LightweightIocContainer.Interfaces;
using LightweightIocContainer.Interfaces.Installers;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Installers;

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
        Installers = [];

        Type[] types = assembly.GetTypes();
        foreach (Type type in types)
        {
            if (!typeof(IIocInstaller).IsAssignableFrom(type) || type.IsNestedPrivate)
                continue;

            Installers.Add(Creator.CreateInstance<IIocInstaller>(type));
        }
    }

    /// <summary>
    /// The <see cref="IIocInstaller"/>s of the Assembly that this <see cref="AssemblyInstaller"/> is installing
    /// </summary>
    public List<IIocInstaller> Installers { get; }

    /// <summary>
    /// Install the found <see cref="IIocInstaller"/>s in the given <see cref="IIocContainer"/>
    /// </summary>
    /// <param name="registration">The <see cref="IRegistrationCollector"/> where <see cref="IRegistration"/>s are added</param>
    public void Install(IRegistrationCollector registration)
    {
        foreach (IIocInstaller installer in Installers)
            installer.Install(registration);
    }
}