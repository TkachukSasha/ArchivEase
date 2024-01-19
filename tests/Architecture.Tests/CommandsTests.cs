using Architecture.Tests.Base;
using SharedKernel.Commands;

namespace Architecture.Tests;

public class CommandsTests : BaseArchitectureTest
{
    [Fact]
    public void Should_Success_WhenAllCommands_InheritICommandInterface_HaveNameEndingWithCommand()
    {
        var result = Types.InAssembly(CoreAssembly)
            .That()
            .ImplementInterface(typeof(ICommand<>))
            .Should()
            .HaveNameEndingWith("Command")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Should_Success_WhenAllCommandHandlers_InheritICommandHandlerInterface_HaveNameEndingWithCommandHandler_And_BeSealed()
    {
        var result = Types.InAssembly(CoreAssembly)
            .That()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .Should()
            .HaveNameEndingWith("CommandHandler")
            .And()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}