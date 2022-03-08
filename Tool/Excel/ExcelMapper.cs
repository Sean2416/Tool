using System;
using System.Collections.Generic;

namespace Tool.Excel
{
    public class ExcelMapper<TSource>
    {
        public List<MapperConfig> MapperConfigs;

        public ExcelMapper()
        {
            MapperConfigs = new List<MapperConfig>();
        }

        public class MapperConfig
        { 
            public string ColumnName { get; set; }

            public Func<TSource, object> Func { get; set; }
        }
    }
}
