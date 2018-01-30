using Emma.Blog.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Emma.Blog.Service.Auth
{
    public class CheckCodeParam
    {
        public bool IsRequired { get; set; }

        public string Code { get; set; }
    }
    public class ValidateCode
    {
        public static RedisClient redisClient = new RedisClient();

        public static bool Check(string clientId, string code)
        {

            string json = redisClient.GetString(clientId+ "_code");
            if (string.IsNullOrEmpty(json))
            {
                return true;
            }
            CheckCodeParam param = JsonHelper.Deserialize<CheckCodeParam>(json);
            return param.Code == code;
        }

        public static bool IsRequired(string clientId)
        {
            string json = redisClient.GetString(clientId+"_code");
            if (string.IsNullOrEmpty(json))
            {
                return false;
            }
            CheckCodeParam param = JsonHelper.Deserialize<CheckCodeParam>(json);
            return param.IsRequired;
        }


        public static string GetCode(string clientId)
        {
            Random random = new Random();
            string code = random.Next(10231, 99999).ToString();
            CheckCodeParam param = new CheckCodeParam();
            param.Code = code;
            param.IsRequired = true;
            redisClient.SetString(clientId+"_code", JsonHelper.Serialize(param), TimeSpan.FromMinutes(5));
            return code;
        }

        public static bool ContainsClientId(string clientId)
        {
            return redisClient.ContainsKey(clientId);
        }

    }
}
