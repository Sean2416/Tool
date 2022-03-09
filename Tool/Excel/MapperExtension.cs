using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Tool.Excel
{
    public static class MapperExtension
    {
        public static ExcelMapper<TSource> Map<TSource>(this ExcelMapper<TSource> mapper, string colName, Func<TSource, object> func)
        {
            mapper.MapperConfigs.Add(new ExcelMapper<TSource>.MapperConfig
            { 
                ColumnName = colName,
                Func = func
            });

            return mapper;
        }
    }
}
