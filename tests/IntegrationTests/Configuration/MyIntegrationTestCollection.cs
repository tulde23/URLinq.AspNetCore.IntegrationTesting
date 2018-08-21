using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ulinq.AspNetCore.IntegrationTesting;
using Xunit;

namespace IntegrationTests.Configuration
{
    [CollectionDefinition(nameof(MyIntegrationTestCollection))]
    public class MyIntegrationTestCollection : AbstractIntegrationTestCollection<MyIntegrationTestFixture,TestStartup>
    {
    }
}
