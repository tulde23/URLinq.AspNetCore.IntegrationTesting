using Microsoft.AspNetCore.Mvc.Testing;
using URLinq.AspNetCore.IntegrationTesting.Fixtures;

namespace URLinq.AspNetCore.IntegrationTesting.Contracts
{
    /// <summary>
    /// Defines an integration test accepting both a collection and class fixture.
    /// </summary>
    /// <typeparam name="TEntryPoint">The type of the entry point.</typeparam>
    /// <typeparam name="TWebApplicationFactory">The type of the web application factory.</typeparam>
    /// <typeparam name="TFixture">The type of the fixture.</typeparam>
    public interface IIntegrationTest<TEntryPoint, TWebApplicationFactory, TFixture>
        where TEntryPoint : class
        where TWebApplicationFactory : WebApplicationFactory<TEntryPoint>
        where TFixture : IIntegrationTestCollectionFixture<TEntryPoint, TWebApplicationFactory>
    {
        /// <summary>
        /// Gets the collection fixture.  This fixture is shared across test classes.
        /// </summary>
        /// <value>
        /// The fixture.
        /// </value>
        TFixture CollectionFixture { get; }

        /// <summary>
        /// Gets the class fixture.  This fixture is shared across tests in a single class.
        /// </summary>
        /// <value>
        /// The class fixture.
        /// </value>
        IntegrationTestClassFixture ClassFixture { get; }
    }
}