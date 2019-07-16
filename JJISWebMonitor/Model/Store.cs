using System;
using System.Collections.Generic;

namespace JJISWebMonitor.Model
{
   public static class Store
   {
      public static DateTimeOffset StartTime;
      public static DateTimeOffset LastCheck;
      private static IList<Outage> _outages;

      static Store()
      {
         ResetOutages();
      }

      public static void AddOutage(Outage outage)
      {
         _outages.Add(outage);
      }

      public static IList<Outage> GetOutages()
      {
         return _outages;
      }

      public static void ResetOutages()
      {
         StartTime = DateTimeOffset.Now;
         _outages = new List<Outage>();
      }
   }
}