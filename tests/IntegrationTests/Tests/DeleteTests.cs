using System.Threading.Tasks;
using IntegrationTests.Configuration;
using TestApi.Controllers;
using ulinq.AspNetCore.IntegrationTesting.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Tests
{
    public class DeleteTests : TestIntegrationTest
    {
        public DeleteTests(MyIntegrationTestFixture fixture,
            IntegrationTestClassFixture integrationClassFixture,
            ITestOutputHelper testOutputHelper = null) : base(fixture, integrationClassFixture, testOutputHelper)
        {
        }

        [Theory]
        [InlineData(1)]
        public async Task DeleteTest(int key)
        {
            await this.ClassFixture.InvokeAsyncVoid<ValuesController>(controller => controller.Delete(key));
        }
    }
}