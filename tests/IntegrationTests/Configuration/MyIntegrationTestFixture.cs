using ulinq.AspNetCore.IntegrationTesting.Attributes;
using ulinq.AspNetCore.IntegrationTesting.Fixtures;

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