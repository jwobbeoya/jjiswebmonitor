using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace JJISWebMonitor.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class MonitorController : ControllerBase
   {
      // GET api/values
      [HttpGet]
      public ActionResult<string> Get()
      {
         var webRequest = (HttpWebRequest) WebRequest.Create(new Uri("https://www.jjis.oregon.gov/staticcontent/connectivitytest.html"));

         var sw = Stopwatch.StartNew();

         var response =  (HttpWebResponse) webRequest.GetResponse();
         using (var sr = new StreamReader(response.GetResponseStream()))
         {
            sr.ReadToEnd();
         }
         
         sw.Stop();

         return $"{sw.ElapsedMilliseconds} ms";
      }
   }
}
