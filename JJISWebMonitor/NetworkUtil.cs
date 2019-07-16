using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using JJISWebMonitor.Model;

namespace JJISWebMonitor
{
   public class NetworkUtil
   {
      public static long MeasureOpenPort(string host, int port = 443, int timeout = 5000)
      {
         using (var client = new TcpClient())
         {
            try
            {
               var task = client.ConnectAsync(host, port);
               var sw = Stopwatch.StartNew();

               if (!task.Wait(timeout))
                  throw new Exception("Timeout opening port");

               sw.Stop();
               return sw.ElapsedMilliseconds;
            }
            finally
            {
               client.Close();
            }
         }
      }

      public static long MeasureHttpRequest(string url, int timeout = 5000)
      {
         var webRequest = (HttpWebRequest) WebRequest.Create(new Uri(url));

         webRequest.Timeout = timeout;
         webRequest.ReadWriteTimeout = timeout;
         webRequest.ContinueTimeout = timeout / 10;

         var sw = Stopwatch.StartNew();
         using (var response = (HttpWebResponse) webRequest.GetResponse())
         {
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

         sw.Stop();
         return sw.ElapsedMilliseconds;
      }
   }
}
