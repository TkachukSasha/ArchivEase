using Architecture.Tests.Base;
using SharedKernel.Queries;

namespace Architecture.Tests;

public class QueriesTests : BaseArchitectureTest
{
    [Fact]
    public void Should_Success_WhenAllQueries_InheritIQueryInterface_HaveNameEndingWithQuery()
    {
        var result = Types.InAssembly(CoreAssembly)
            .That()
            .ImplementInterface(typeof(IQuery<>))
            .Should()
            .HaveNameEndingWith("Query")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Should_Success_WhenAllQueryHandlers_InheritIQueryHandlerInterface_HaveNameEndingWithQueryHandler_And_BeSealed()
    {
        var result = Types.InAssembly(CoreAssembly)
            .That()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .Should()
            .HaveNameEndingWith("QueryHandler")
            .And()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}