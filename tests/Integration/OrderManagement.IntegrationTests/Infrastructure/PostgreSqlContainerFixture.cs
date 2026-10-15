using Testcontainers.PostgreSql;
using Testcontainers.Xunit;
using Xunit.Abstractions;

namespace OrderManagement.IntegrationTests.Infrastructure;

public sealed class PostgreSqlContainerFixture
    : ContainerFixture<PostgreSqlBuilder, PostgreSqlContainer>
{
    public PostgreSqlContainerFixture(IMessageSink messageSink) : base(messageSink) { }

    protected override PostgreSqlBuilder Configure()
    {
        return new PostgreSqlBuilder("postgres:18");
    }
}
