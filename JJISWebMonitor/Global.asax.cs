using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using System.Web.Http;
using JJISWebMonitor.Model;

namespace JJISWebMonitor
{
   public class WebApiApplication : System.Web.HttpApplication
   {
      private static string domain = "www.jjis.oregon.gov";
      private static readonly Timer Timer = new Timer(30000);
      private const int Timeout = 800;

      protected void Application_Start()
      {
         GlobalConfiguration.Configure(WebApiConfig.Register);
         Timer.Elapsed += Timer_Elapsed;
         Timer.Start();
      }

      private void Timer_Elapsed(object sender, ElapsedEventArgs e)
      {
         OpenPort();
         HttpRequest();
      }

      private static void OpenPort()
      {
         try
         {
            using (var client = new TcpClient())
            {
               var task = client.ConnectAsync(domain, 443);

               if (!task.Wait(200))
               {
                  Store.AddOutage(new Outage("Timeout opening port"));
               }
            }
         }
         catch (Exception ex)
         {
            LogException(ex);
         }
      }

      private static void HttpRequest()
      {
         try
         {
            Store.LastCheck = DateTimeOffset.Now;

            var webRequest =
               (HttpWebRequest)WebRequest.Create(
                  new Uri($"https://{domain}/staticcontent/connectivitytest.html?{DateTime.Now.Ticks}"));

            webRequest.Timeout = Timeout;
            webRequest.ReadWriteTimeout = Timeout;
            webRequest.ContinueTimeout = Timeout / 10;

            var response = (HttpWebResponse)webRequest.GetResponse();
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
            LogException(ex);
         }
      }

      private static void LogException(Exception ex)
      {
         Store.AddOutage(new Outage(ex.Message));
         Trace.WriteLine(ex.Message);
      }

   }
}
