// // Author: Gockner, Simon
// // Created: 2019-06-12
// // Copyright(c) 2019 SimonG. All Rights Reserved.

using System.Collections.Generic;

namespace LightweightIocContainer.Interfaces.Installers
{
    public interface IAssemblyInstaller : IIocInstaller
    {
        List<IIocInstaller> Installers { get; }
    }
}