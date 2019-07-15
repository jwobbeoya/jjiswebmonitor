using System;
using System.Collections.Generic;
using System.Web.Http;
using JJISWebMonitor.Model;

namespace JJISWebMonitor.Controllers
{
   public class OutageController : ApiController
   {
      [HttpGet]
      //[Route("List")]
      public IList<Outage> List()
      {
         return Store.GetOutages();
      }

      [HttpGet]
      public DateTimeOffset StartTime()
      {
         return Store.StartTime;
      }

      [HttpGet]
      public bool Reset()
      {
         Store.ResetOutages();
         return true;
      }
   }
}
