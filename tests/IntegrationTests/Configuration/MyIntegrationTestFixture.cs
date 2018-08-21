using URLinq.AspNetCore.IntegrationTesting.Attributes;
using URLinq.AspNetCore.IntegrationTesting.Fixtures;

[assembly: SolutionContentRoot("tests/TestApi")]

namespace IntegrationTests.Configuration
{
    public class MyIntegrationTestFixture : IntegrationTestCollectionFixture<TestStartup>
    {
        public MyIntegrationTestFixture()
        {
        }
    }
}