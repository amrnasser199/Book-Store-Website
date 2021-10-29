using Book_Store.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var WebHost = CreateWebHostBuilder(args).Build();
            RunMigration(WebHost);

            WebHost.Run();
        }

        private static void RunMigration(IWebHost webHost)
        {
            using(var Scope=webHost.Services.CreateScope())
            {
                var db = Scope.ServiceProvider.GetRequiredService<BookStoreDbcontext>();
                db.Database.Migrate();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
