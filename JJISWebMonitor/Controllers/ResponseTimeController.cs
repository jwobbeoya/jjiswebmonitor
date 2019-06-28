using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JJISWebMonitor.Controllers
{
    public class ResponseTimeController : ApiController
    {
       public string GetResponseTime()
       {
         var webRequest = (HttpWebRequest)WebRequest.Create(new Uri("https://www.jjis.oregon.gov/staticcontent/connectivitytest.html"));

         var sw = Stopwatch.StartNew();

         var response = (HttpWebResponse)webRequest.GetResponse();
         using (var responseStream = response.GetResponseStream())
         {
            if (responseStream == null)
               return $"Response was null.  Response: {response.StatusCode} {response.StatusDescription}";

            using (var sr = new StreamReader(responseStream))
            {
               sr.ReadToEnd();
            }

         }

         sw.Stop();

         return $"{sw.ElapsedMilliseconds} ms";
      }
    }
}
