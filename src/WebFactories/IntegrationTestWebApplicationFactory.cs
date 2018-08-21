using System;
using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using ulinq.AspNetCore.IntegrationTesting.Attributes;

namespace ulinq.AspNetCore.IntegrationTesting
{
    public class IntegrationTestWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
    {
        protected override void ConfigureClient(HttpClient client)
        {
            base.ConfigureClient(client);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            Assembly a = typeof(TEntryPoint).Assembly;
            var attribute = a.GetCustomAttribute<SolutionContentRootAttribute>();
            if (attribute == null)
            {
                throw new ArgumentNullException("You integration test assembly is missing the SolutionContentRootAttribute.  Please decorate your assembly [assembly:SolutionContentRoot('path to your content root)");
            }
            builder.UseSolutionRelativeContentRoot(attribute.Path);
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder().UseStartup<TEntryPoint>();
        }
    }
}