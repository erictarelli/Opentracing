using Microsoft.Extensions.Logging;
using OpenTracing;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using OpenTracing.Propagation;
using OpenTracing.Tag;

namespace ConsoleApp.Example.Http
{
    internal class Program
    {
        private readonly ITracer _tracer;
        private readonly WebClient _webclient = new WebClient();
        public Program(ITracer tracer)
        {
            _tracer = tracer;
        }

        public async void SayHello(string helloTo)
        {
            using (var scope = _tracer.BuildSpan("say-hello").StartActive(true))
            {
                var helloString = await FormatString(helloTo);

                Console.WriteLine(helloString);
            }
        }

        private async Task<string> FormatString(string helloTo)
        {
            var url = $"https://localhost:5001/api/alumnos";

            using (var scope = _tracer.BuildSpan("HTTP GET")
                .WithTag(Tags.HttpMethod, "GET")
                .WithTag(Tags.HttpUrl, $"{url}")
                .StartActive(true))
            {
                
                var helloString = string.Empty;

                try
                {
                    helloString = _webclient.DownloadString(url);
                }
                catch (Exception ex)
                {

                    helloString = ex.ToString();
                }
               

                scope.Span.Log(new Dictionary<string, object>
                {
                    [LogFields.Event] = "string.Format",
                    ["value"] = helloString
                });

                return helloString;
            }
        }

        public static void Main(string[] args)
        {
            //if (args.Length != 1)
            //{
            //    throw new ArgumentException("Expecting one argument");
            //}

            //using (var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole()))
            //{
                //var helloTo = args[0];

                //var logger = loggerFactory.CreateLogger<Program>();

                using (var tracer = Tracing.InitTracer("app-console"))
                {
                //new Program(tracer).SayHello("Eric");
                using (var scope = tracer.BuildSpan("say-hello").StartActive(true))
                {
                    //var helloString = await FormatString(helloTo);

                    //Console.WriteLine(helloString);
                }
            }

            //}
        }
    }
}
