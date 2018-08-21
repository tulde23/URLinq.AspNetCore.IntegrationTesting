using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using URLinq.AspNetCore.IntegrationTesting.Contracts;
using URLinq.AspNetCore.IntegrationTesting.Logging;
using Xunit.Abstractions;

namespace URLinq.AspNetCore.IntegrationTesting.Fixtures
{
    public class IntegrationTestClassFixture : IIntegrationTestClassFixture
    {
        public ITestLogger Logger { get; private set; }

        public HttpClient Client { get; private set; }

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
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">expression</exception>
        public HttpRequestMessage CreateHttpRequestMessage<TController, TResponse>(
             Expression<Func<TController, TResponse>> expression) where TController : ControllerBase
        {
            var message = RouteHelper.BuildRequestMessage(expression);
            // Logger.Write($"Invoking URL {message.RequestUri.ToString()}");
            return message;
        }

        /// <summary>
        /// Invokes an IControllerAction
        /// </summary>
        /// <typeparam name="TController">The type of the ControllerBase.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="client">The http client.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">controllerAction</exception>
        public async Task<TResponse> InvokeAsyncWithResults<TController, TResponse>(
            Expression<Func<TController, TResponse>> expression) where TController : ControllerBase
        {
            var message = RouteHelper.BuildRequestMessage(expression);
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
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">controllerAction</exception>
        public async Task<TResponse> InvokeAsyncWithResults<TController, TResponse>(
            Expression<Func<TController, object>> expression) where TController : ControllerBase
        {
            var response = await Client.SendAsync(CreateHttpRequestMessage(expression));
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
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">expression</exception>
        public TResponse InvokeAsyncTask<TController, TResponse>(Expression<Func<TController, TResponse>> expression) where TResponse : Task where TController : ControllerBase
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
            var response = DispatchRequest(Client, expression).GetAwaiter().GetResult();
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
        /// <returns></returns>
        private async Task<string> DispatchRequest<TController, TResponse>(HttpClient client, Expression<Func<TController, TResponse>> expression) where TController : ControllerBase
        {
            var message = CreateHttpRequestMessage(expression);
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
        /// <returns></returns>
        public async Task InvokeAsyncVoid<TController>(Expression<Func<TController, object>> expression) where TController : ControllerBase
        {
            var message = CreateHttpRequestMessage(expression);
            var response = await Client.SendAsync(message);
            response.EnsureSuccessStatusCode();
        }
    }
}