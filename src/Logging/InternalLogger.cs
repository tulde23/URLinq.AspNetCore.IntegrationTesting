using URLinq.AspNetCore.IntegrationTesting.Contracts;
using Xunit.Abstractions;

namespace URLinq.AspNetCore.IntegrationTesting.Logging
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
            TestOutputHelper?.WriteLine(message);
        }
    }
}