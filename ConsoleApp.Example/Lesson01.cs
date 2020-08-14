using Microsoft.Extensions.Logging;
using OpenTracing;
using System;
using System.Collections.Generic;

namespace ConsoleApp.Example
{
    internal class Lesson01
    {
        private readonly ITracer _tracer;
        private readonly ILogger<Lesson01> _logger;

        public Lesson01(ITracer tracer, ILogger<Lesson01> logger)
        {
            _tracer = tracer;
            _logger = logger;
        }

        private string FormatString(ISpan rootSpan, string helloTo)
        {
            using (var scope = _tracer.BuildSpan("format-string").AsChildOf(rootSpan).StartActive(true))
            {
                var helloString = $"Hello, {helloTo}!";
                scope.Span.Log(new Dictionary<string, object>
                {
                    [LogFields.Event] = "string.Format",
                    ["value"] = helloString
                });

                return helloString;
            }                                   
        }

        private void PrintHello(ISpan rootSpan, string helloString)
        {
            using (var scope = _tracer.BuildSpan("print-hello").AsChildOf(rootSpan).StartActive(true))
            {
                _logger.LogInformation(helloString);
                scope.Span.Log("WriteLine");
            }            
        }

        public void SayHello(string helloTo)
        {
            using (var scope = _tracer.BuildSpan("say-hello").StartActive(true))
            {
                var helloString = FormatString(scope.Span, helloTo);

                PrintHello(scope.Span, helloString);

                Console.WriteLine(helloString);
            }
        }
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                throw new ArgumentException("Expecting one argument");
            }

            using (var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole()))
            {
                var helloTo = args[0];

                var logger = loggerFactory.CreateLogger<Lesson01>();

                using (var tracer = Tracing.InitTracer("hello-world", loggerFactory))
                {
                    new Lesson01(tracer, logger).SayHello(helloTo);
                }
                
            }
        }
    }
}
