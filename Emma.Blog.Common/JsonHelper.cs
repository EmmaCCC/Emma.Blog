using System;
using Newtonsoft.Json;

namespace Emma.Blog.Common
{
    public class JsonHelper
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);

            }
            catch(Exception ex)
            {
                throw ex;
            }
           
        }

        public static dynamic Deserialize<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);

            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
     
    }
}
