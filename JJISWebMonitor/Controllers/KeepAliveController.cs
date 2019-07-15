using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JJISWebMonitor.Controllers
{
   public class KeepAliveController : ApiController
   {
      [HttpGet]
      public string Index()
      {
         return "OK";
      }
   }
}
