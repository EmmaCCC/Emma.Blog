using System;
using System.Collections.Concurrent;
using System.IO;
using log4net;
using log4net.Config;
using log4net.Repository;


namespace Emma.Blog.Common
{
   
    public class LogHelper
    {
        private static ILog logger;
        static LogHelper()
        {
     
            ILoggerRepository repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
            logger = LogManager.GetLogger(repository.Name, "NETCorelog4net");
        }
 

        public static void Error(object message, Exception ex)
        {
            logger.Error(message, ex);
        }


        public static void Error(object message)
        {
            logger.Error(message);
        }

        public static void Info(object message, Exception ex)
        {
            logger.Error(message, ex);
        }

        public static void Info(object message)
        {
            logger.Info(message);
        }




    }
}
