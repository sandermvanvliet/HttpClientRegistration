using System.Net.Http;
using System.Reflection;
using Serilog;

namespace http
{
    public static class HttpClientExtensions
    {
        private static readonly FieldInfo HandlerField = typeof(HttpMessageInvoker).GetField("_handler", BindingFlags.Instance | BindingFlags.NonPublic);

        public static void DumpHandlers(this HttpClient httpClient, ILogger logger)
        {
            var handler = HandlerField.GetValue(httpClient) as HttpMessageHandler;

            if (!(handler is DelegatingHandler))
            {
                logger.Information("Handler {Type} => {HashCode}", handler.GetType().FullName, handler.GetHashCode());
            }

            var innerHandler = handler as DelegatingHandler;
            while (innerHandler != null)
            {
                logger.Information("Inner Handler {Type} => {HashCode}", innerHandler.GetType().FullName, innerHandler.GetHashCode());

                if (innerHandler.InnerHandler is DelegatingHandler)
                {
                    innerHandler = innerHandler.InnerHandler as DelegatingHandler;
                }
                else
                {
                    logger.Information("Handler {Type} => {HashCode}", innerHandler.InnerHandler.GetType().FullName, innerHandler.InnerHandler.GetHashCode());
                    break;
                }
            }
        }
    }
}