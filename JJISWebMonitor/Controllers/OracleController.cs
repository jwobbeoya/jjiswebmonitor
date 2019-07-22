using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Oracle.ManagedDataAccess.Client;

namespace JJISWebMonitor.Controllers
{
    public class OracleController : ApiController
    {
       [HttpGet]
       public string Index([FromUri] string connectionString)
       {
          try
          {
             using (var connection = new OracleConnection(connectionString))
             {
                connection.Open();
                connection.Close();
                connection.Dispose();
             }
             return "Success";
          }
          catch (Exception ex)
          {
             return ex.Message;
          }
       }
    }
}
