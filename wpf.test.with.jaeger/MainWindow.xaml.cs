using OpenTracing;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public MainWindow()
        {
            using (var tracer = Tracing.InitTracer("wpf-test"))
            {
                using (var scope = tracer.BuildSpan("say-hello").StartActive(true))
                {
                    //var helloString = await FormatString(helloTo);

                    //Console.WriteLine(helloString);
                }
                //new Program(tracer).SayHello("Eric");
            }
            //_tracer = Tracing.InitTracer("wpf-test");
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var scope = _tracer.BuildSpan("simple-test").StartActive(true))
            { 
                Console.WriteLine("iam testing jaeger");
            }
        }
    }
}
