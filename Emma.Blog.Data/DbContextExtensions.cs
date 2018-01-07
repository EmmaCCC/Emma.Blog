using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

 namespace Microsoft.EntityFrameworkCore
{
    public static class DbContextExtensions
    {
        public static IQueryable<T> PageAsQuery<T, TKey>(this DbContext dbContext,int pageIndex, int pageSize, out int totalCount, out int pageCount,
            Expression<Func<T, bool>> whereLambda, bool isAsc, Expression<Func<T, TKey>> orderBy) where T : class, new()
        {
            IQueryable<T> temp = dbContext.Set<T>().Where(whereLambda).AsQueryable();
            totalCount = temp.Count();
            pageSize = Math.Min(pageSize, 30);
            pageCount = (int)Math.Ceiling((double)totalCount / pageSize);

            if (isAsc)
            {
                temp = temp.OrderBy(orderBy)
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize).AsQueryable();
            }
            else
            {
                temp = temp.OrderByDescending(orderBy)
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize).AsQueryable();
            }
            return temp.AsNoTracking();
        }


        public static IQueryable<T> PageAsQuery<T, TKey>(this DbContext dbContext,IQueryable<T> query, int pageIndex, int pageSize, out int totalCount, out int pageCount, bool isAsc, Expression<Func<T, TKey>> orderBy) where T : class, new()
        {
            IQueryable<T> temp = query;
            totalCount = temp.Count();
            pageSize = Math.Min(pageSize, 30);
            pageCount = (int)Math.Ceiling((double)totalCount / pageSize);
            if (isAsc)
            {
                temp = temp.OrderBy(orderBy)
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize).AsQueryable();
            }
            else
            {
                temp = temp.OrderByDescending(orderBy)
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize).AsQueryable();
            }
            return temp;
        }


        public static bool Exist<T>(this DbContext dbContext,Expression<Func<T, bool>> anyLambda) where T : class, new()
        {
            return dbContext.Set<T>().Any(anyLambda);
        }

        public static T Find<T>(this DbContext dbContext,params object[] keyValues) where T : class, new()
        {
            return dbContext.Set<T>().Find(keyValues);
        }


        public static int Count<T>(this DbContext dbContext,Expression<Func<T, bool>> countLambda) where T : class, new()
        {
            return dbContext.Set<T>().Count(countLambda);
        }

        public static T First<T>(this DbContext dbContext,Expression<Func<T, bool>> firstLambda) where T : class, new()
        {
            return dbContext.Set<T>().FirstOrDefault(firstLambda);
        }



        public static int SaveChanges(this DbContext dbContext)
        {
            try
            {
               return dbContext.SaveChanges();
            }

            catch (DbUpdateException ex)
            {

                //Common.LogHelper.WriteLog(this.GetType(), ex.InnerException == null ? ex : ex.InnerException.InnerException);
                throw;
            }
            catch (Exception ex)
            {
                //Common.LogHelper.WriteLog(this.GetType(), ex);
                throw;
            }

        }
    }
}
