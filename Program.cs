using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;
using WebApiContrib.Formatting.Jsonp;   

namespace JasmineReceiver
{
    class Program
    {
        public static System.Threading.ManualResetEvent shutdown = new ManualResetEvent(false);
        public static int returnCode = -999;

        static void Main(string[] args)
        {
            var config = new HttpSelfHostConfiguration("http://192.168.0.6:8181");

            FormatterConfig.RegisterFormatters(config.Formatters);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { controller = "logger", action = "get", format = "jsonp" }
            );


            using (HttpSelfHostServer server = new HttpSelfHostServer(config))
            {
                try
                {
                    Console.WriteLine("Waiting ... ");
                    server.OpenAsync().Wait();
                    shutdown.WaitOne(5 * 60 * 1000);    // wait for 5 minutes
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                Console.WriteLine("Returning - " + returnCode);
                System.Environment.Exit(returnCode);
            }

        }
    }
}
