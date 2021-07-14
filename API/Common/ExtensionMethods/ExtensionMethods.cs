using API.Controllers._Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace API.Common.ExtensionMethods
{
    public static class ExtensionMethods
    {
        private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }

        private static bool TryGetLambda<T>(string propertyName, out Expression<Func<T, object>> expression)
        {
            try
            {
                expression = ToLambda<T>(propertyName);
                return true;
            }
            catch (Exception)
            {
                expression = null;
                return false;
            }
        }

        public static async Task<Pagination.Response<TResult>> PagingAsync<TSource, TResult>(
            this IQueryable<TSource> query,
            Pagination.Request request,
            Expression<Func<TSource, TResult>> selector)
        {
            var count = await query.CountAsync();

            //order
            if (!string.IsNullOrWhiteSpace(request.OrderBy))
            {
                if (TryGetLambda(request.OrderBy, out Expression<Func<TSource, object>> expression))
                {
                    query = request.OrderDescending ? query.OrderByDescending(expression).AsQueryable() : query.OrderBy(expression).AsQueryable();
                }
            }

            //select
            var items = await query
            .Skip(request.PageSize * (request.Page - 1))
            .Take(request.PageSize)
            .Select(selector)
            .ToListAsync();

            return new Pagination.Response<TResult>
            {
                Total = count,
                Items = items,
            };
        }
    }
}