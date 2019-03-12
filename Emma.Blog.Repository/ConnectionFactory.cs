using System;
using System.Data;
using System.Data.SqlClient;
using Emma.Blog.Common;

namespace Emma.Blog.Repository
{
    public class ConnectionFactory
    {
        public static IDbConnection SqlServerFactory(IServiceProvider provider)
        {
            var con = new SqlConnection(Global.Configuration.GetSection("ConnectionStrings:SqlServerConnection").Value);
            return con;
        }
     
    }


    
}
