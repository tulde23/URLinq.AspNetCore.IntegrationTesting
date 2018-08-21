using System;
using System.Collections.Generic;
using System.Text;

namespace ulinq.AspNetCore.IntegrationTesting.Contracts
{
    /// <summary>
    /// A common logger
    /// </summary>
    public interface ITestLogger
    {
        /// <summary>
        /// Writes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Write(string message);
    }
}
