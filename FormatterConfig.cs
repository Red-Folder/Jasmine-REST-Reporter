using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using WebApiContrib.Formatting.Jsonp;

namespace JasmineReceiver
{
    class FormatterConfig
    {
        public static void RegisterFormatters(MediaTypeFormatterCollection formatters)
        {
            var jsonFormatter = formatters.JsonFormatter;
            jsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            // Insert the JSONP formatter in front of the standard JSON formatter.
            var jsonpFormatter = new JsonpMediaTypeFormatter(formatters.JsonFormatter);
            formatters.Insert(0, jsonpFormatter);
        }
    }
}
