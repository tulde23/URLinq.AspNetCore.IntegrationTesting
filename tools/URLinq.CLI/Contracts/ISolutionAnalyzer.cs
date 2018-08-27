using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using URLinq.CLI.Models;

namespace URLinq.CLI.Contracts
{
    /// <summary>
    /// A Roslyn analyzer
    /// </summary>
    public interface ISolutionAnalyzer
    {
        Task<IEnumerable<ProjectModel>> Analyze(string solutionPath);
    }
}
