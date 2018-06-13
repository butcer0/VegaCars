using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VegaCars.Core.Models;

namespace VegaCars.Extensions
{
    public static class IQueryableExtension
    {
        public static IQueryable<T> ApplyingOrdering<T>(this IQueryable<T> query, IQueryObject queryObject, Dictionary<string, Expression<Func<T, object>>> columnsMap)
        {
            if(string.IsNullOrWhiteSpace(queryObject.SortBy) ||  !columnsMap.ContainsKey(queryObject.SortBy))
            {
                return query;
            }

            if (queryObject.IsSortAscending)
            {
                return query.OrderBy(columnsMap[queryObject.SortBy]);
            }
            else
            {
                return query.OrderByDescending(columnsMap[queryObject.SortBy]);
            }
        }

        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, IQueryObject queryObject)
        {
            if (queryObject.PageSize <= 0)
            {
                queryObject.PageSize = 10;
            }

            if (queryObject.Page <= 0)
            {
                queryObject.Page = 1;
            }

            return query.Skip((queryObject.Page - 1) * queryObject.PageSize).Take(queryObject.PageSize);
        }
    }
}
