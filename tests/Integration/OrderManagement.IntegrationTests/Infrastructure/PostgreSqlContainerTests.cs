namespace OrderManagement.IntegrationTests.Infrastructure;

public sealed class PostgreSqlContainerTests
    : IClassFixture<PostgreSqlContainerFixture>
{
    private readonly PostgreSqlContainerFixture _fixture;

    public PostgreSqlContainerTests(PostgreSqlContainerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task PostgreSql_ShouldExeceuteSql()
    {
        var connectionString =
            _fixture.Container.GetConnectionString();

        var result = await _fixture.Container
            .ExecScriptAsync("SELECT 1");

        Assert.Equal(0L, result.ExitCode);
        Assert.Empty(result.Stderr);
    }
}
