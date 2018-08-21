using ulinq.AspNetCore.IntegrationTesting;
using ulinq.AspNetCore.IntegrationTesting.Fixtures;
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