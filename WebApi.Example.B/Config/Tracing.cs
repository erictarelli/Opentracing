using Jaeger;
using Jaeger.Samplers;
using Microsoft.Extensions.Logging;
using OpenTracing.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Example.B.Config
{
    public static class Tracing
    {
        public static void InitTracer(string serviceName)
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


            GlobalTracer.Register((Tracer)new Configuration(serviceName, loggerFactory)
                .WithSampler(samplerConfiguration)
                .WithReporter(reporterConfiguration)
                .GetTracer());
        }
    }
}