using System.Threading.Tasks;
using IntegrationTests.Configuration;
using TestApi.Controllers;
using URLinq.AspNetCore.IntegrationTesting.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Tests
{
    public class PutTests : TestIntegrationTest
    {
        public PutTests(MyIntegrationTestFixture fixture,
            IntegrationTestClassFixture integrationClassFixture,
            ITestOutputHelper testOutputHelper = null) : base(fixture, integrationClassFixture, testOutputHelper)
        {
        }

        [Theory]
        [InlineData(1, "1")]
        public async Task PutTest(int id, string key)
        {
            await this.ClassFixture.InvokeAsyncVoid<ValuesController>(controller => controller.Put(id, key));
        }
    }
}