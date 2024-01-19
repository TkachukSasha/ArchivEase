using Architecture.Tests.Base;
using SharedKernel.Dal;

namespace Architecture.Tests;

public class InitializersTests : BaseArchitectureTest
{
    [Fact]
    public void Should_Success_WhenAllInitializers_ImplementInitializerInterface()
    {
        var result = Types.InAssembly(CoreAssembly)
            .That()
            .ImplementInterface(typeof(IDataInitializer))
            .Should()
            .HaveNameEndingWith("Initializer")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Should_Success_WhenAllInitializers_AreSealedAnd_ImplementInitializerInterface()
    {
        var result = Types.InAssembly(CoreAssembly)
            .That()
            .ImplementInterface(typeof(IDataInitializer))
            .Should()
            .HaveNameEndingWith("Initializer")
            .And()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}