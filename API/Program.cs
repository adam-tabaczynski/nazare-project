using System;
using Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence;

namespace API
{
  public class Program
  {
    public static void Main(string[] args)
    {
      // CreateHostBuilder(args).Build().Run();
      // We changed that, so automatically when the app starts running and there is no DB set up, the new one will be created.
      var host = CreateHostBuilder(args).Build();

      // normally, we would use a DI - BUT, we want to use that just once to create DB, and not anytime later on - 'using' statement allows that.
      // After the first use, that scope and what it contains will be automatically disposed.
      using (var scope = host.Services.CreateScope())
      {
        // That allows to getting Services we needed - DataContext and ILogger.
        var services = scope.ServiceProvider;
        try
        {
          // check tip on Migrate() - that method creates a database if none exist.
          var context = services.GetRequiredService<DataContext>();
          var userManager = services.GetRequiredService<UserManager<AppUser>>();
          context.Database.Migrate();

          // When application starts/restarts and there are no activites in DB, this will automatically seed the DB with previously prepared
          // generic activites.
          // getting basic users from UserManager is async - I cannot run it in Main,
          // so I use .Wait(). 
          Seed.SeedData(context, userManager).Wait();
        }
        catch (Exception ex)
        {
          // Here we take another GetService to precisely get the error message.
          var logger = services.GetRequiredService<ILogger<Program>>();
          logger.LogError(ex, "An error occured during migration");
        }
      }

      // Due to the fact that host = CreateHostBuilder(args).Build(), the line below is equal to original one.
      host.Run();

    }

    // .ConfigureDefaultBuilder and .ConfigureWebHostDefaults - to see what they do, check their definition (F12).
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
            });
  }
}
