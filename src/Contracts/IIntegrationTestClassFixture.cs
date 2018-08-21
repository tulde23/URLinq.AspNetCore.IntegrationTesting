using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;

namespace ulinq.AspNetCore.IntegrationTesting.Contracts
{
    /// <summary>
    /// Defines a class fixture.  Class fixtures are shared amongst tests within the same test class.
    /// </summary>
    public interface IIntegrationTestClassFixture
    {
        /// <summary>
        /// Gets the logger.  Loggers are scoped per class, hence the class fixture designation.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        ITestLogger Logger { get; }

        /// <summary>
        /// Gets the client.  The HttpClient is shared across test classes.  It will be initialized within a collection.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        HttpClient Client { get; }

        /// <summary>
        /// Initializes the class fixture.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <param name="client">The client.</param>
        void Bootstrap(ITestOutputHelper testOutputHelper, HttpClient client);

        /// <summary>
        /// Creates the HTTP request message.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        HttpRequestMessage CreateHttpRequestMessage<TController, TResponse>(
             Expression<Func<TController, TResponse>> expression) where TController : ControllerBase;

        /// <summary>
        /// Invokes a controller action async.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        Task<TResponse> InvokeAsyncWithResults<TController, TResponse>(
            Expression<Func<TController, TResponse>> expression) where TController : ControllerBase;

        /// <summary>
        /// Invokes a controller action async.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        Task<TResponse> InvokeAsyncWithResults<TController, TResponse>(
            Expression<Func<TController, object>> expression) where TController : ControllerBase;

        /// <summary>
        /// Invokes a controller action async.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        Task InvokeAsyncVoid<TController>(
            Expression<Func<TController, object>> expression) where TController : ControllerBase;

    }
}