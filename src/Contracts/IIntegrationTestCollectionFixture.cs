using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using URLinq.AspNetCore.IntegrationTesting.Contracts;
using Xunit.Abstractions;

namespace URLinq.AspNetCore.IntegrationTesting
{
    /// <summary>
    /// Defines a common test fixture.
    /// </summary>
    public interface IIntegrationTestCollectionFixture<TEntryPoint, TWebApplicationFactory> : IDisposable
        where TEntryPoint : class
        where TWebApplicationFactory : WebApplicationFactory<TEntryPoint>
    {
        TWebApplicationFactory Factory { get; }
        HttpClient Client { get; }
   

        /// <summary>
        /// Bootstraps this instance.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        void Bootstrap();
    }
}