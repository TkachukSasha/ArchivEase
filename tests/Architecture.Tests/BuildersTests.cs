using Architecture.Tests.Base;
using SharedKernel.Builders.Base;

namespace Architecture.Tests;

public class BuildersTests : BaseArchitectureTest
{
    [Fact]
    public void Should_Success_WhenAllEncodeBuilders_InheritFrom_BaseEncoder()
    {
        var result = Types.InAssembly(CoreAssembly)
            .That()
            .Inherit(typeof(BaseEncodeBuilder<,,>))
            .Should()
            .HaveNameEndingWith("EncodeBuilder")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Should_Success_WhenAllDecodeBuilders_InheritFrom_BaseDecoder()
    {
        var result = Types.InAssembly(CoreAssembly)
            .That()
            .Inherit(typeof(BaseDecodeBuilder<,,>))
            .Should()
            .HaveNameEndingWith("DecodeBuilder")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Should_Success_WhenAllBuilders_AreSealed()
    {
        var result = Types.InAssembly(CoreAssembly)
            .That()
            .HaveNameEndingWith("Builder")
            .And()
            .AreClasses()
            .Should()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}