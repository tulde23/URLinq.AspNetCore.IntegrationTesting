using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using URLinq.AspNetCore.IntegrationTesting.Contracts;
using URLinq.AspNetCore.IntegrationTesting.Logging;
using Xunit.Abstractions;

namespace URLinq.AspNetCore.IntegrationTesting.Fixtures
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="URLinq.AspNetCore.IntegrationTesting.Contracts.IIntegrationTestClassFixture" />
    public class IntegrationTestClassFixture : IIntegrationTestClassFixture
    {
        /// <summary>
        /// Gets the logger.  Loggers are scoped per class, hence the class fixture designation.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public ITestLogger Logger { get; private set; }

        /// <summary>
        /// Gets the client.  The HttpClient is shared across test classes.  It will be initialized within a collection.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        public HttpClient Client { get; private set; }

        /// <summary>
        /// Initializes the class fixture.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <param name="client">The client.</param>
        public void Bootstrap(ITestOutputHelper testOutputHelper, HttpClient client)
        {
            Logger = new InternalLogger(testOutputHelper);
            Client = client;
        }

        /// <summary>
        /// Creates the HTTP request message from a lambda expression
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="headerBuilder">The header builder.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">expression</exception>
        public HttpRequestMessage CreateHttpRequestMessage<TController, TResponse>(
             Expression<Func<TController, TResponse>> expression, Action<HttpRequestHeaders> headerBuilder = null) where TController : ControllerBase
        {
            var message = RouteHelper.BuildRequestMessage(expression);
            // Logger.Write($"Invoking URL {message.RequestUri.ToString()}");
            headerBuilder?.Invoke(message.Headers);
            return message;
        }

        /// <summary>
        /// Invokes an IControllerAction
        /// </summary>
        /// <typeparam name="TController">The type of the ControllerBase.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="headerBuilder">The header builder.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">controllerAction</exception>
        public async Task<TResponse> InvokeAsyncWithResults<TController, TResponse>(
            Expression<Func<TController, TResponse>> expression, Action<HttpRequestHeaders> headerBuilder = null) where TController : ControllerBase
        {
            var message = CreateHttpRequestMessage(expression,headerBuilder);
            var response = await Client.SendAsync(message);
            var dataAsString = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            try
            {
                return JsonConvert.DeserializeObject<TResponse>(dataAsString);
            }
            catch
            {
                return (TResponse)Convert.ChangeType(dataAsString, typeof(TResponse));
            }
        }

        /// <summary>
        /// Invokes an IControllerAction
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="headerBuilder">The header builder.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">controllerAction</exception>
        public async Task<TResponse> InvokeAsyncWithResults<TController, TResponse>(
            Expression<Func<TController, object>> expression, Action<HttpRequestHeaders> headerBuilder = null) where TController : ControllerBase
        {
            var response = await Client.SendAsync(CreateHttpRequestMessage(expression,headerBuilder));
            var dataAsString = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            var name = typeof(TResponse).FullName;
            try
            {
                return JsonConvert.DeserializeObject<TResponse>(dataAsString);
            }
            catch
            {
                return (TResponse)Convert.ChangeType(dataAsString, typeof(TResponse));
            }
        }

        /// <summary>
        /// Invokes an IControllerAction that returns a generic Task.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="headerBuilder">The header builder.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">expression</exception>
        public TResponse InvokeAsyncTask<TController, TResponse>(Expression<Func<TController, TResponse>> expression, Action<HttpRequestHeaders> headerBuilder = null) where TResponse : Task where TController : ControllerBase
        {
            if (expression == null)
            {
                throw new System.ArgumentNullException(nameof(expression));
            }
            var responseType = typeof(TResponse);
            //if generic, get the actual type
            if (responseType.IsGenericType)
            {
                responseType = responseType.GetGenericArguments()[0];
            }
            var response = DispatchRequest(Client, expression,headerBuilder).GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject(response, responseType);
            var method = typeof(Task).GetMethod("FromResult");
            var genericMethod = method.MakeGenericMethod(responseType);
            return (TResponse)genericMethod.Invoke(null, new object[] { result });
        }

        /// <summary>
        /// Dispatches the request.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="client">The client.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="headerBuilder">The header builder.</param>
        /// <returns></returns>
        private async Task<string> DispatchRequest<TController, TResponse>(HttpClient client, Expression<Func<TController, TResponse>> expression, Action<HttpRequestHeaders> headerBuilder = null) where TController : ControllerBase
        {
            var message = CreateHttpRequestMessage(expression,headerBuilder);
            var response = await client.SendAsync(message);
            var dataAsString = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            return dataAsString;
        }

        /// <summary>
        /// Invokes a controller action async.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="headerBuilder">The header builder.</param>
        /// <returns></returns>
        public async Task InvokeAsyncVoid<TController>(Expression<Func<TController, object>> expression, Action<HttpRequestHeaders> headerBuilder = null) where TController : ControllerBase
        {
            var message = CreateHttpRequestMessage(expression,headerBuilder);
            var response = await Client.SendAsync(message);
            response.EnsureSuccessStatusCode();
        }
    }
}