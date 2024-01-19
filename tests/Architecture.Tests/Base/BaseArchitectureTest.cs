using Core;
using System.Reflection;

namespace Architecture.Tests.Base;

public abstract class BaseArchitectureTest
{
    internal protected Assembly CoreAssembly => typeof(AssemblyReference).Assembly;
}
