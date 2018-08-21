using System.Threading.Tasks;
using IntegrationTests.Configuration;
using TestApi.Controllers;
using ulinq.AspNetCore.IntegrationTesting.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Tests
{
    public class PostTests : TestIntegrationTest
    {
        public PostTests(MyIntegrationTestFixture fixture, IntegrationTestClassFixture integrationClassFixture, ITestOutputHelper testOutputHelper = null) :
            base(fixture, integrationClassFixture, testOutputHelper)
        {
        }

        [Theory(DisplayName = "Post Test")]
        [InlineData("1")]
        public async Task PostTest(string data)
        {
            await this.ClassFixture.InvokeAsyncVoid<ValuesController>(controller => controller.Post(data));
        }
    }
}