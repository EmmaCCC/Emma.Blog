using System.Collections.Generic;
using System.Data;
using Dapper.Contrib.Extensions;

namespace Emma.Blog.Repository
{
    public class BaseRepository<T> where T : class
    {
        IDbConnection con;

        public BaseRepository(IDbConnection con)
        {
            this.con = con;
        }
        public long Insert(T entity)
        {
            return con.Insert(entity);
        }

        public long Insert(List<T> entity)
        {
            return con.Insert(entity);
        }

        public T Get(object id)
        {
            return con.Get<T>(id);
        }

        public bool Update(T entity)
        {
            return con.Update(entity);
        }

        public bool Delete(T entity)
        {
            return con.Delete(entity);
        }
    }
}
