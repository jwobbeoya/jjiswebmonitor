using System;
using System.Runtime.Serialization;

namespace JJISWebMonitor.Model
{
   [DataContract]
   public class Outage
   {
      public Outage(string message)
      {
         TimeStamp = DateTimeOffset.Now;
         Message = message;
      }

      [DataMember]
      public DateTimeOffset TimeStamp { get; set; }

      [DataMember]
      public string Message { get; set; }
   }
}