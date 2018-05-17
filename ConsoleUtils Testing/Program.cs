using ConsoleUtils.Testing;

namespace ConsoleUtilsTesting
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            LoggingAndWritingTest loggingAndWritingTest = new LoggingAndWritingTest();
            loggingAndWritingTest.Test();

            AsyncTCPTest asyncTCPTest = new AsyncTCPTest();
            asyncTCPTest.Test();
        }
    }
}