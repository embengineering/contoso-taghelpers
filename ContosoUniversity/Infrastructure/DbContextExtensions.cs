using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Infrastructure
{
    public static class DbContextExtensions
    {
        public static IQueryable Set(this DbContext dbContext, Type t)
        {
            return (IQueryable)dbContext.GetType().GetMethod("Set")?.MakeGenericMethod(t).Invoke(dbContext, null);
        }
    }
}
