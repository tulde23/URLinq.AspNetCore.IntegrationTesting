using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;

namespace URLinq.AspNetCore.IntegrationTesting.Fixtures
{
    /// <summary>
    /// A default implementation of an xunit collection fixture.
    /// </summary>
    /// <typeparam name="TEntryPoint">The type of the entry point.</typeparam>
    /// <typeparam name="TWebApplicationFactory">The type of the web application factory.</typeparam>
    /// <seealso cref="URLinq.AspNetCore.IntegrationTesting.IIntegrationTestCollectionFixture{TEntryPoint, TWebApplicationFactory}" />
    public class IntegrationTestCollectionFixture<TEntryPoint, TWebApplicationFactory> :
        IIntegrationTestCollectionFixture<TEntryPoint, TWebApplicationFactory>
          where TEntryPoint : class
        where TWebApplicationFactory : WebApplicationFactory<TEntryPoint>
    {
        public TWebApplicationFactory Factory { get; private set; }
        public HttpClient Client { get; private set; }

        public void Bootstrap()
        {
            Factory = Activator.CreateInstance<TWebApplicationFactory>();
            Client = Factory.CreateClient();
        }

        public void Dispose()
        {
            if (Client != null)
            {
                Client.Dispose();
            }
            if (Factory != null)
            {
                Factory.Dispose();
            }
        }
    }

    /// <summary>
    /// A default implementation of an xunit fixture that uses a default implementation of TWebApplicationFactory
    /// </summary>
    public class IntegrationTestCollectionFixture<TEntryPoint> :
        IntegrationTestCollectionFixture<TEntryPoint, IntegrationTestWebApplicationFactory<TEntryPoint>>
          where TEntryPoint : class
    {
    }
}