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
         //Timer.Start();
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
            NetworkUtil.MeasureOpenPort(domain, 443, 200);
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
            NetworkUtil.MeasureHttpRequest($"https://{domain}/staticcontent/connectivitytest.html?{DateTime.Now.Ticks}", 500);
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
