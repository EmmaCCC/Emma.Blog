using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Snowflake.Core;

namespace Emma.Blog.Common
{
    public class SnowId
    {
        public static IdWorker Worker;

        static SnowId()
        {


            string appSettingBasePath = AppDomain.CurrentDomain.BaseDirectory;
            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile(appSettingBasePath + "appsettings.json")
                .Build();

            long workerId = Convert.ToInt64(configBuilder.GetSection("SnowId:WorkerId").Value);
            long dataCenterId = Convert.ToInt64(configBuilder.GetSection("SnowId:DataCenterId").Value);
            Worker = new IdWorker(workerId, dataCenterId);

        }

        public static long NewId()
        {
            return Worker.NextId();
        }
    }
}
