using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ulinq.AspNetCore.IntegrationTesting.Contracts;

namespace ulinq.AspNetCore.IntegrationTesting.Fixtures
{
    public static class IntegrationTestClassFixtureExtensions
    {
        /// <summary>
        /// Invoke an API method that returns a collection of items
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="client">The client.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static async Task<IEnumerable<TResponse>> ManyAsync<TController, TResponse>(this IIntegrationTestClassFixture client, Expression<Func<TController, object>> expression) where TController : ControllerBase
        {
            return await client.InvokeAsyncWithResults<TController, IEnumerable<TResponse>>(expression);
        }

        /// <summary>
        /// Invoke an API method that returns a single item
        /// of items
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="client">The client.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static async Task<TResponse> SingleAsync<TController, TResponse>(this IIntegrationTestClassFixture client, Expression<Func<TController, object>> expression) where TController : ControllerBase
        {
            return await client.InvokeAsyncWithResults<TController, TResponse>(expression);
        }
    }
}