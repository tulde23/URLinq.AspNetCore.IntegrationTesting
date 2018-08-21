using ulinq.AspNetCore.IntegrationTesting.Contracts;
using Xunit.Abstractions;

namespace ulinq.AspNetCore.IntegrationTesting.Logging
{
    internal class InternalLogger : ITestLogger
    {
        public InternalLogger(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;
        }

        public ITestOutputHelper TestOutputHelper { get; }

        public void Write(string message)
        {
           // TestOutputHelper?.WriteLine(message);
        }
    }
}