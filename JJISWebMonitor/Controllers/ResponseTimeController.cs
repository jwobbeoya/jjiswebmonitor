﻿using System;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace JJISWebMonitor.Controllers
{
   public class ResponseTimeController : ApiController
   {
      public const int MaxConnections = 100;

      [HttpGet]
      public string Http([FromUri] string uri, int timeout = 5000)
      {
         try
         {
            return $"{NetworkUtil.MeasureHttpRequest(uri, timeout)} ms";
         }
         catch (Exception ex)
         {
            return $"{ex.Message}";
         }
      }

      [HttpGet]
      public async Task<string> Connect([FromUri] string host, [FromUri] int port = 443, int timeout = 5000)
      {
         try
         {
            return $"{await NetworkUtil.MeasureOpenPort(host, port, timeout)} ms";
         }
         catch (Exception ex)
         {
            var currentException = ex;
            var sb = new StringBuilder();
            while (currentException != null)
            {
               sb.Append(currentException.Message + " ");
               currentException = currentException.InnerException;
            }

            return sb.ToString();
         }
      }

      [HttpGet]
      public async Task<string> AverageConnect([FromUri] string host, [FromUri] int port = 443, int connections = 10, int timeout = 5000)
      {
         connections = Math.Min(connections, MaxConnections);
         var totalTime = 0F;
         var successCount = 0;
         var timeouts = 0;
         var error = string.Empty;

         for (var i = 0; i < connections; i++)
         {
            try
            {

               var time = await NetworkUtil.MeasureOpenPort(host, port, timeout);
               totalTime += time;
               successCount++;
            }

            catch (TimeoutException ex)
            {
               timeouts++;
               error = ex.Message;
            }
            catch (Exception ex)
            {
               error = ex.Message;
            }
         }

         if (successCount == 0)
            return $"All connections failed. {error}";

         var average = Math.Round(totalTime / successCount, 2);
         var timeoutText = timeouts == 1
            ? "timeout"
            : "timeouts";

         var timeoutMessage = timeouts == 0
            ? string.Empty
            : $"with {timeouts} {timeoutText}";

         return $"Average connect time was {average} ms {timeoutMessage}";
      }
   }
}