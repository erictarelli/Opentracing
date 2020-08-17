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

        public void FormatString(string helloTo)
        {
            var url = $"https://localhost:5001/api/examblea";

            using (var scope = _tracer.BuildSpan("HTTP GET").StartActive(true))
            {
                scope.Span
                    .SetTag(Tags.SpanKind, Tags.SpanKindClient)
                    .SetTag(Tags.HttpMethod, "GET")
                    .SetTag(Tags.HttpUrl, url);

                var dictionary = new Dictionary<string, string>();

                _tracer.Inject(scope.Span.Context, BuiltinFormats.HttpHeaders, new TextMapInjectAdapter(dictionary));

                foreach (var entry in dictionary)
                    _webclient.Headers.Add(entry.Key, entry.Value);

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
            }
        }

        public static void Main(string[] args)
        {
            using (var tracer = Tracing.InitTracer("app-console"))
            {
                using (var scope = tracer.BuildSpan("say-hello").StartActive(true))
                {
                    new Program(tracer).FormatString("test");
                }
            }
        }
    }
}
