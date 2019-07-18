using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Sockets;
using System.Web;
using JJISWebMonitor.Model;

namespace JJISWebMonitor
{
   public class NetworkUtil
   {
      private const int MaxTimeout = 30000;

      public static long MeasureOpenPort(string host, int port = 443, int timeout = 5000)
      {
         timeout = Math.Min(timeout, MaxTimeout);

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
         timeout = Math.Min(timeout, MaxTimeout);

         var webRequest = CreateWebRequest(url, timeout);

         var sw = new Stopwatch();
         try
         {
            sw.Start();
            using (var response = (HttpWebResponse) webRequest.GetResponse())
            {
               using (var responseStream = response.GetResponseStream())
               {
                  using (var sr = new StreamReader(responseStream))
                  {
                     var content = sr.ReadToEnd();

                     sw.Stop();

                     if (sw.ElapsedMilliseconds > timeout)
                        throw new Exception("The operation has timed out.");

                     if (content.Length < 1)
                        throw new Exception("Empty Response");
                  }

               }
            }
         }
         catch (WebException ex)
         {
            if (ex.Status == WebExceptionStatus.RequestCanceled)
               throw new Exception("The operation has timed out.");

            throw;
         }

         return sw.ElapsedMilliseconds;
      }

      public static HttpWebRequest CreateWebRequest(string url, int timeout)
      {
         var webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));

         webRequest.Timeout = timeout;
         webRequest.ReadWriteTimeout = timeout;
         webRequest.ContinueTimeout = timeout / 10;
         webRequest.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
         webRequest.KeepAlive = false;

         return webRequest;
      }
   }
}
