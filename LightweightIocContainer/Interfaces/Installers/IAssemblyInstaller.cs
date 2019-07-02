// // Author: Gockner, Simon
// // Created: 2019-06-12
// // Copyright(c) 2019 SimonG. All Rights Reserved.

using System.Collections.Generic;
using System.Reflection;

namespace LightweightIocContainer.Interfaces.Installers
{
    /// <summary>
    /// An <see cref="IIocInstaller"/> that installs all <see cref="IIocInstaller"/>s for its given <see cref="Assembly"/>
    /// </summary>
    public interface IAssemblyInstaller : IIocInstaller
    {
        /// <summary>
        /// The <see cref="IIocInstaller"/>s of the <see cref="Assembly"/> that this <see cref="IAssemblyInstaller"/> is installing
        /// </summary>
        List<IIocInstaller> Installers { get; }
    }
}