using Jaeger;
using Jaeger.Samplers;
using Microsoft.Extensions.Logging;

namespace ConsoleApp.Example.Http
{
    public static class Tracing
    {
        public static Tracer InitTracer(string serviceName)
        {
            var loggerFactory = new LoggerFactory();

            var senderConfig = new Configuration.SenderConfiguration(loggerFactory)
                    .WithAgentHost("localhost")
                    .WithAgentPort(6831);

            var samplerConfiguration = new Configuration.SamplerConfiguration(loggerFactory)
                .WithType(ConstSampler.Type)
                .WithParam(1);

            var reporterConfiguration = new Configuration.ReporterConfiguration(loggerFactory)
                .WithLogSpans(true)
                .WithSender(senderConfig);

            return (Tracer)new Configuration(serviceName, loggerFactory)
                .WithSampler(samplerConfiguration)
                .WithReporter(reporterConfiguration)
                .GetTracer();
        }
    }
}
