using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntegrationTests.Configuration;
using Microsoft.AspNetCore.Mvc.Testing;
using TestApi.Controllers;
using URLinq.AspNetCore.IntegrationTesting.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Tests
{
    public class GetTests : TestIntegrationTest
    {
        public GetTests(MyIntegrationTestFixture fixture,
            IntegrationTestClassFixture integrationClassFixture,
            ITestOutputHelper testOutputHelper = null) : base(fixture, integrationClassFixture, testOutputHelper)
        {
        }

        [Fact]
        public async Task GetListOfValues()
        {
            var response = await ClassFixture.ManyAsync<ValuesController, string>(controller => controller.Get());
            Assert.NotNull(response);
            Assert.True(response.Count() > 0);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetSingleValue(int key)
        {
            var response = await ClassFixture.SingleAsync<ValuesController, string>(controller => controller.Get(key));
            Assert.NotNull(response);
        }
    }
}