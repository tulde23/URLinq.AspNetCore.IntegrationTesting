using Microsoft.AspNetCore.Mvc.Testing;
using ulinq.AspNetCore.IntegrationTesting.Contracts;
using ulinq.AspNetCore.IntegrationTesting.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace ulinq.AspNetCore.IntegrationTesting
{
    /// <summary>
    /// An abstract integration test class that uses an xunit collection fixture and class fixture.
    /// A collection fixture is used to shared the WebFactory and HttpClient across all test classes, while the class fixture is used to shared state across the test.
    /// All implementations (test classes) must decorate themselves with
    /// the [Collection("NameOfCollection")] class attribute.
    /// </summary>
    /// <typeparam name="TEntryPoint">The type of the entry point.</typeparam>
    /// <typeparam name="TWebApplicationFactory">The type of the web application factory.</typeparam>
    /// <typeparam name="TFixture">The type of the fixture.</typeparam>
    /// <seealso cref="ulinq.AspNetCore.IntegrationTesting.Contracts.IIntegrationTest{TEntryPoint, TWebApplicationFactory, TFixture}" />
    public abstract class AbstractIntegrationTest<TEntryPoint, TWebApplicationFactory, TFixture> :
        IIntegrationTest<TEntryPoint, TWebApplicationFactory, TFixture>,
        IClassFixture<IntegrationTestClassFixture>
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
        public TFixture CollectionFixture { get; }

        /// <summary>
        /// Gets the class fixture.  This fixture is shared across tests in a single class.
        /// </summary>
        /// <value>
        /// The class fixture.
        /// </value>
        public IntegrationTestClassFixture ClassFixture
        {
            get;
        }

        public AbstractIntegrationTest(TFixture collectionFixture, IntegrationTestClassFixture integrationClassFixture, ITestOutputHelper testOutputHelper = null)
        {
            CollectionFixture = collectionFixture;
            CollectionFixture.Bootstrap();
            ClassFixture = integrationClassFixture;
            ClassFixture.Bootstrap(testOutputHelper, CollectionFixture.Client);
        }
    }

    /// <summary>
    /// An abstract integration test class that uses an xunit collection fixture and class fixture.
    /// A collection fixture is used to shared the WebFactory and HttpClient across all test classes, while the class fixture is used to shared state across the test.
    /// All implementations (test classes) must decorate themselves with
    /// the [Collection("NameOfCollection")] class attribute.
    /// </summary>
    /// <typeparam name="TEntryPoint">The type of the entry point.</typeparam>
    /// <typeparam name="TFixture">The type of the fixture.</typeparam>
    /// <seealso cref="ulinq.AspNetCore.IntegrationTesting.Contracts.IIntegrationTest{TEntryPoint, TWebApplicationFactory, TFixture}" />
    public abstract class AbstractIntegrationTest<TEntryPoint, TFixture> :
        AbstractIntegrationTest<TEntryPoint, IntegrationTestWebApplicationFactory<TEntryPoint>, TFixture>,
        IClassFixture<IntegrationTestClassFixture>
        where TEntryPoint : class
        where TFixture : IIntegrationTestCollectionFixture<TEntryPoint, IntegrationTestWebApplicationFactory<TEntryPoint>>
    {
        public AbstractIntegrationTest(TFixture collectionFixture, IntegrationTestClassFixture integrationClassFixture, ITestOutputHelper testOutputHelper = null)
            : base(collectionFixture, integrationClassFixture, testOutputHelper)
        {
        }
    }
}