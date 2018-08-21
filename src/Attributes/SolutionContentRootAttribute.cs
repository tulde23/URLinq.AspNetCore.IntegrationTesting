using System;
using System.Collections.Generic;
using System.Text;

namespace ulinq.AspNetCore.IntegrationTesting.Attributes
{
    /// <summary>
    /// Sets the solution content root for an integration test assembly.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Assembly)]
    public class SolutionContentRootAttribute : Attribute
    {
        public string Path { get; set; }
        public SolutionContentRootAttribute(string path)
        {
            this.Path = path;
        }
    }
}
