using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Sockets;
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
            Console.WriteLine("Jasmine REST Receiver - https://github.com/Red-Folder/Jasmine-REST-Reporter");
            Console.WriteLine("Run with --help for usage");
            Console.WriteLine();

            // Set up our initial vaiables
            String ipAddress = LocalIPAddress().ToString();
            String port = "8181";
            int secondsToWait = 10 * 60;    // 10 minutes
            Boolean helpOnly = false;

            // See we have parameters passed in
            foreach (var arg in args)
            {
                // User wants usage instructions
                if (arg.StartsWith("--help", StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.WriteLine("Usage: JasmineReceiver [options]");
                    Console.WriteLine("Options:");
                    Console.WriteLine("--help - this usage message");
                    Console.WriteLine("--ipaddress={ipaddress} - set ip address to listen on");
                    Console.WriteLine("--port={port} - port to listen on");
                    Console.WriteLine("--timeout={seconds} - number of seconds to wait for communication before timing out");

                    helpOnly = true;
                    returnCode = 0;
                }

                if (arg.StartsWith("--ipaddress", StringComparison.CurrentCultureIgnoreCase))
                    ipAddress = arg.Split('=')[1];

                if (arg.StartsWith("--port", StringComparison.CurrentCultureIgnoreCase))
                    port = arg.Split('=')[1];

                if (arg.StartsWith("--timeout", StringComparison.CurrentCultureIgnoreCase))
                    secondsToWait = int.Parse(arg.Split('=')[1]);

            }

            if (!helpOnly)
            {

                String url = "http://" + ipAddress + ":" + port;
                Console.WriteLine("Attempting to listen on {0}", url);
                var config = new HttpSelfHostConfiguration(url);

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
                        Console.WriteLine("Opening server ... ");
                        server.OpenAsync().Wait();
                        Console.WriteLine("Waiting ... ");
                        shutdown.WaitOne(secondsToWait * 1000);
                    }
                    catch (System.AggregateException exceptions)
                    {
                        exceptions.Handle((x) =>
                        {
                            // We know this is a lack of permissions
                            if (x is System.ServiceModel.AddressAccessDeniedException) // This we know how to handle.
                            {
                                Console.WriteLine("Permissions need to allow this application to run on port {0}", port);
                                Console.WriteLine("Either run as Administrator or run the below from the command line");
                                Console.WriteLine("");
                                Console.WriteLine("netsh http add urlacl url=http://+:{0}/ user=\"{1}\"", port, System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                                Console.WriteLine("");
                                Console.WriteLine("If you get \"Url reservation add failed, Error: 183\", then delete the current reservation using:");
                                Console.WriteLine("");
                                Console.WriteLine("netsh http delete urlacl url=http://+:{0}/", port);
                                return true;
                            }
                            return false; // Let anything else stop the application.
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }

                }

                Console.WriteLine("Returning - " + returnCode);
                System.Environment.Exit(returnCode);
            }

        }

        private static IPAddress LocalIPAddress() {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                return null;
            }

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            return host
                .AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }
    }
}
