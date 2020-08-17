using OpenTracing;
using OpenTracing.Propagation;
using OpenTracing.Tag;
using OpenTracing.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wpf.test.with.jaeger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ITracer _tracer;
        public MainWindow(ITracer tracer)
        {
            InitializeComponent();
            _tracer = tracer;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            var url = $"https://localhost:5001/api/examplea";

            using (var scope = _tracer.BuildSpan("HTTP GET").StartActive(true))
            {
                var helloString = string.Empty;

                try
                {
                    HttpClient client = new HttpClient();

                    scope.Span
                        .SetTag(Tags.SpanKind, Tags.SpanKindClient)
                        .SetTag(Tags.HttpMethod, "GET")
                        .SetTag(Tags.HttpUrl, url);

                    var dictionary = new Dictionary<string, string>();

                    _tracer.Inject(scope.Span.Context, BuiltinFormats.HttpHeaders, new TextMapInjectAdapter(dictionary));

                    foreach (var entry in dictionary)
                        client.DefaultRequestHeaders.Add(entry.Key, entry.Value);



                    var response = await client.GetAsync(url);
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
    }
}
