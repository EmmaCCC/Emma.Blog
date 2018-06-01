using Emma.Blog.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Emma.Blog.Data
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
