using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using TestApi;

namespace IntegrationTests.Configuration
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
