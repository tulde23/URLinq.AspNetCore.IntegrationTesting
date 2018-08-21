using URLinq.AspNetCore.IntegrationTesting;
using URLinq.AspNetCore.IntegrationTesting.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Configuration
{
    [Collection(nameof(MyIntegrationTestCollection))]
    public class TestIntegrationTest : AbstractIntegrationTest<TestStartup, MyIntegrationTestFixture>
    {
        public TestIntegrationTest(MyIntegrationTestFixture fixture,
            IntegrationTestClassFixture integrationClassFixture,
            ITestOutputHelper testOutputHelper = null) : base(fixture,integrationClassFixture,testOutputHelper)
        {
        }
    }
}