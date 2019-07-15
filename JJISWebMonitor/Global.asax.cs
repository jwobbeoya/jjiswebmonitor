using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Timers;
using System.Web.Http;
using JJISWebMonitor.Model;

namespace JJISWebMonitor
{
   public class WebApiApplication : System.Web.HttpApplication
   {
      private static readonly Timer Timer = new Timer(30000);
      private const int Timeout = 1000;

      protected void Application_Start()
      {
         GlobalConfiguration.Configure(WebApiConfig.Register);
         Timer.Elapsed += Timer_Elapsed;
         Timer.Start();
      }

      private void Timer_Elapsed(object sender, ElapsedEventArgs e)
      {
         try
         {
            var webRequest =
               (HttpWebRequest) WebRequest.Create(
                  new Uri("https://www.jjis.oregon.gov/staticcontent/connectivitytest.html"));

            webRequest.Timeout = Timeout;
            webRequest.ReadWriteTimeout = Timeout;
            webRequest.ContinueTimeout = Timeout / 10;

            var response = (HttpWebResponse) webRequest.GetResponse();
            using (var responseStream = response.GetResponseStream())
            {
               if (responseStream == null)
                  throw new Exception(
                     $"Response was null.  Response: {response?.StatusCode} {response?.StatusDescription}");

               using (var sr = new StreamReader(responseStream))
               {
                  if (sr.ReadToEnd().Length < 1)
                     throw new Exception("Empty Response");
               }
            }
         }
         catch (Exception ex)
         {
            Store.AddOutage(new Outage(ex.Message));
            Trace.WriteLine(ex.Message);
         }
      }

   }
}
