using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Web.Http;

namespace JasmineReceiver
{
    public class LoggerController : ApiController
    {

        [HttpGet, HttpPost]
        public string SuiteResult(int numberOfSpecs, int numberOfFails)
        {
            Console.WriteLine("Number of specs: " + numberOfSpecs + ", fails: " + numberOfFails);


            Console.WriteLine("=======================================================================");
            if (numberOfSpecs == 0 || numberOfFails > 0)
            {
                Console.WriteLine("FAILURE");
                Program.returnCode = -1;
            }
            else
            {
                Console.WriteLine("SUCCESSS");
                Program.returnCode = 0;
            }
            Console.WriteLine("=======================================================================");

            Program.shutdown.Set();

            return "Thanks from Suite Result";
        }

        
        [HttpGet, HttpPost]
        public string SpecResult(string suiteDescription, string specDescription, string resultText)
        {
            Console.WriteLine("Suite: " + suiteDescription + ", Spec: " + specDescription + ", Result:" + resultText);

            return "Thanks from Spec Result";
        }

    }
}
