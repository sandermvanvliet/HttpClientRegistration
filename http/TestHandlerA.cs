using Serilog;

namespace http
{
    public class TestHandlerA : TestHandlerBase
    {
        public TestHandlerA(ILogger logger) : base(logger)
        {
        }
    }
}