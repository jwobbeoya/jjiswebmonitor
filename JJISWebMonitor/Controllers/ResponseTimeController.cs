using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Web.Http;

namespace JJISWebMonitor.Controllers
{
    public class ResponseTimeController : ApiController
    {
       public string GetResponseTime()
       {
         var webRequest = (HttpWebRequest)WebRequest.Create(new Uri("https://www.jjis.oregon.gov/staticcontent/connectivitytest.html"));

         var sw = Stopwatch.StartNew();

         try
         {
            var response = (HttpWebResponse)webRequest.GetResponse();
            using (var responseStream = response.GetResponseStream())
            {
               if (responseStream == null)
                  return $"Response was null.  Response: {response?.StatusCode} {response?.StatusDescription}";

               using (var sr = new StreamReader(responseStream))
               {
                  Trace.WriteLine(sr.ReadToEnd());
               }

               sw.Stop();
            }
         }
         catch (WebException ex)
         {
            return $"{ex.Message}";
         }


         return $"{sw.ElapsedMilliseconds} ms";
      }
    }
}
