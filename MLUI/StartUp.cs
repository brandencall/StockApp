using DataAccessLibrary.Data;
using DataAccessLibrary.Databases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MLUI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLUI
{
    public static class StartUp
    {
        public static IConfiguration config { get; set; }
        public static ServiceProvider serviceProvider;
        public static void OnStartUp()
        {
            var services = new ServiceCollection();
            services.AddTransient<DataAccessService>();
            services.AddTransient<DataCollectionService>();
            services.AddTransient<ISqlDataAccess, SqlDataAccess>();
            services.AddTransient<ISqlDataML, SqlDataML>();

            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json");

            config = builder.Build();
            services.AddSingleton(config);

            serviceProvider = services.BuildServiceProvider();
        }
    }
}
