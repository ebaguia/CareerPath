using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace BasicManipulation
{
    public static class Logger
    {
        private static log4net.ILog Log { get; set; }

        static Logger()
        {
            Log = log4net.LogManager.GetLogger(typeof(Logger));
        }

        public static void Error(object msg)
        {
            Log.Error(msg);
        }

        public static void Error(object msg, Exception ex)
        {
            Log.Error(msg, ex);
        }

        public static void Error(Exception ex)
        {
            Log.Error(ex.Message, ex);
        }

        public static void Fatal(object msg)
        {
            Log.Error(msg);
        }

        public static void Fatal(object msg, Exception ex)
        {
            Log.Error(msg, ex);
        }

        public static void Fatal(Exception ex)
        {
            Log.Error(ex.Message, ex);
        }

        public static void Debug(object msg)
        {
            Log.Debug(msg);
        }

        public static void Info(object msg)
        {
            Log.Info(msg);
        }

        public static void Warn(object msg)
        {
            Log.Warn(msg);
        }
    }
}
