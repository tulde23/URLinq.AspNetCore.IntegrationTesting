using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using URLinq.CLI.Models;

namespace URLinq.CLI.Contracts
{
    /// <summary>
    /// Defines an abstraction for generating an integration test from a corresponding Web API Controller
    /// </summary>
    public interface IIntegrationTestGenerator
    {
        Task Generate(ControllerModel controllerModel);
        /// <summary>
        /// Write the in-memory integration test to disk.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        Task WriteTo(string path);
    }
}
