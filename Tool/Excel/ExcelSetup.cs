using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tool.Excel
{
    public static class ExcelSetup
    {
        public static IServiceCollection AddExcelService(this IServiceCollection services)
        {
            var list = GetInheritedClasses();

            foreach (var service in list)
                services.AddScoped(service);

            return services;
        }

        private static IEnumerable<Type> GetInheritedClasses()
        {
            
            return Assembly.GetAssembly(typeof(ExcelService)).GetTypes().Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType.IsSubclassOf(typeof(ExcelService)));

        }
    }
}
