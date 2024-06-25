using DataAccessLibrary.Data;
using DataAccessLibrary.Databases;
using DataInsertScript.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInsertScript
{
    public static class Startup
    {
        public static IConfiguration config {  get; set; }
        public static ServiceProvider serviceProvider;
        public static void OnStartUp()
        {
            var services = new ServiceCollection();

            services.AddTransient<EdgarApiService>();
            services.AddTransient<FMPService>();
            services.AddTransient<DataAccessService>();
            services.AddTransient<ISqlDataAccess, SqlDataAccess>();
            services.AddTransient<ISqlData, SqlData>();

            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json");

            config = builder.Build();
            services.AddSingleton(config);

            serviceProvider = services.BuildServiceProvider();
        }
    }
}
