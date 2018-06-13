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

    }
}
