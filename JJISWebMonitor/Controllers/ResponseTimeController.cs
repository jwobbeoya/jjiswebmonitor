using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Web.Http;

namespace JJISWebMonitor.Controllers
{
   public class ResponseTimeController : ApiController
   {
      public string GetResponseTime([FromUri] string uri)
      {     
         try
         {
            return $"{MeasureRequestTime(uri)} ms";
         }
         catch (Exception ex)
         {
            return $"{ex.Message}";
         }
      }

      private long MeasureRequestTime(string uri)
      {
         var webRequest = (HttpWebRequest)WebRequest.Create(new Uri(uri));
         var sw = Stopwatch.StartNew();
         var response = (HttpWebResponse)webRequest.GetResponse();
         using (var responseStream = response.GetResponseStream())
         {
            if (responseStream == null)
               throw new Exception($"Response was null.  Response: {response?.StatusCode} {response?.StatusDescription}");

            using (var sr = new StreamReader(responseStream))
            {
               Trace.WriteLine(sr.ReadToEnd());
            }

            sw.Stop();
         }

         return sw.ElapsedMilliseconds;
      }

   }
}
