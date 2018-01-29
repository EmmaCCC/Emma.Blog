using System;
using StackExchange.Redis;

namespace Emma.Blog.Common
{
    /// <summary>
    /// redis相关
    /// </summary>
    public class RedisClient : IDisposable
    {
        static string SERVER = "127.0.0.1";
        IDatabase _db;

        private static readonly object Locker = new object();
        private static ConnectionMultiplexer _instance;
        private static ConnectionMultiplexer RedisMultiplexer {
            get
            {
                if (_instance == null)
                {
                    lock (Locker)
                    {
                        if (_instance == null || !_instance.IsConnected)
                        {
                            _instance = GetManager();
                        }
                    }
                }
                return _instance;
            }
        }


        private static ConnectionMultiplexer GetManager()
        {
            try
            {
                ConfigurationOptions options = new ConfigurationOptions();
                options.EndPoints.Add(SERVER);
                options.Password = "1";
                var connect = ConnectionMultiplexer.Connect(options);
                connect.PreserveAsyncOrder = false;
                return connect;
            }
            catch (Exception ex )
            {

                throw ex;
            }
        }
        public RedisClient(int database = 4)
        {
            _db = RedisMultiplexer.GetDatabase(database);
        }


        public void SetString(string key, string value, TimeSpan? timeout = null)
        {
            _db.StringSet(key, value, timeout);
           
        }

        public void Remove(string key)
        {
            _db.KeyDelete(key);
        }

        public string GetString(string key)
        {
            return _db.StringGet(key);
        }
        public long SetList(string key, RedisValue[] values)
        {
            return _db.ListRightPush(key, values);
        }

        public RedisValue[] GetList(string key)
        {
            return _db.ListRange(key);
        }

        public long ListRemove(string key, RedisValue value)
        {
            return _db.ListRemove(key, value);
        }

        public bool ContainsKey(string key)
        {
            return _db.KeyExists(key);
        }

        public void Dispose()
        {
            RedisMultiplexer.Close();
            RedisMultiplexer.Dispose();
            GC.Collect();
        }

        public void Close()
        {
            RedisMultiplexer.Close();
        }
    }
}