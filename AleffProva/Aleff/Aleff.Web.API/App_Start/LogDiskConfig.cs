using System;
using System.IO;
using System.Web.Http;

namespace Aleff.Web.API
{
    public static class LogDiskConfig
  {
        public static void Create(HttpConfiguration config)
        {
          string logsPath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
          if(!Directory.Exists(logsPath))
            Directory.CreateDirectory(logsPath);
        }
    }
}