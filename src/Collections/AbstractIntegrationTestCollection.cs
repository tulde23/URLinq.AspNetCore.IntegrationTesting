using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ulinq.AspNetCore.IntegrationTesting
{
    /// <summary>
    /// A marker class denoting a test collection and binding it to a specific fixture.  All implementors must
    /// decorate themselves with the class attribute [CollectionDefinition("some name")]
    /// </summary>
    public abstract class AbstractIntegrationTestCollection<TFixture, TEntryPoint, TWebApplicationFactory> :
        ICollectionFixture<TFixture>
         where TEntryPoint : class
        where TWebApplicationFactory : WebApplicationFactory<TEntryPoint>
        where TFixture : class, IIntegrationTestCollectionFixture<TEntryPoint, TWebApplicationFactory>
    {
    }

    /// <summary>
    /// A marker class denoting a test collection and binding it to a specific fixture.  All implementors must
    /// decorate themselves with the class attribute [CollectionDefinition("some name")]
    /// This class used the default web application factory.
    /// </summary>
    public abstract class AbstractIntegrationTestCollection<TFixture, TEntryPoint> :
        ICollectionFixture<TFixture>
         where TEntryPoint : class
        where TFixture : class, IIntegrationTestCollectionFixture<TEntryPoint, IntegrationTestWebApplicationFactory<TEntryPoint>>
    {
    }
}