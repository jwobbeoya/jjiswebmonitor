using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Web.Http;

namespace JJISWebMonitor.Controllers
{
   public class ResponseTimeController : ApiController
   {
      [HttpGet]
      public string Http([FromUri] string uri)
      {     
         try
         {
            return $"{NetworkUtil.MeasureHttpRequest(uri)} ms";
         }
         catch (Exception ex)
         {
            return $"{ex.Message}";
         }
      }

      [HttpGet]
      public string Connect([FromUri] string host, [FromUri] int port = 443, int timeout = 5000)
      {
         try
         {
            return $"{NetworkUtil.MeasureOpenPort(host, port, timeout)} ms";
         }
         catch (Exception ex)
         {
            return $"{ex.Message}";
         }
      }
   }
}
