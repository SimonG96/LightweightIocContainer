using JetBrains.Annotations;
using Test.LightweightIocContainer.FactoryGenerator.TestClasses.Interfaces;

namespace Test.LightweightIocContainer.FactoryGenerator.TestClasses.Factories;

[UsedImplicitly]
public interface IAFactory
{
    IA Create();
}