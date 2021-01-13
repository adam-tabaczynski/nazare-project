using System;
using System.Threading.Tasks;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using Application.Activities;
using FluentValidation.AspNetCore;
using API.Middleware;
using Domain;
using Microsoft.AspNetCore.Identity;
using Application.Interfaces;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using AutoMapper;
using Infrastructure.Photos;
using API.SignalR;
using Application.Profiles;

namespace API
{
  public class Startup
  {
    // Injecting configuration into startup class. With that I can access various appsettings.
    // (These appsettings are here)
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureDevelopmentServices(IServiceCollection services)
    {
      services.AddDbContext<DataContext>(opt =>
      {
        // Getting the ConnectionStrings (check tip over GetConnectionString) which are called DefaultConnection.
        // Taken from appsettings.json
        opt.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));

        // Allows using Lazy Loading.
        opt.UseLazyLoadingProxies();
      });

      ConfigureServices(services);
    }

    public void ConfigureProductionServices(IServiceCollection services)
    {
      services.AddDbContext<DataContext>(opt =>
      {
        // Getting the ConnectionStrings (check tip over GetConnectionString) which are called DefaultConnection.
        // Taken from appsettings.json
        opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));

        // Allows using Lazy Loading.
        opt.UseLazyLoadingProxies();
      });

      ConfigureServices(services);
    }
    // This method gets called by the runtime. Use this method to add services to the container.
    // Here I add any Dependency Injection services that I will need to use anywhere in the application.
    public void ConfigureServices(IServiceCollection services)
    {
      // This service addition is required so I can DI DataContext for various DB querying.
      // Here the order in which the services are added plays no role - we can add it however we feel.
      services.AddDbContext<DataContext>(opt =>
      {
        // Getting the ConnectionStrings (check tip over GetConnectionString) which are called DefaultConnection.
        // Taken from appsettings.json
        opt.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));

        // Allows using Lazy Loading.
        opt.UseLazyLoadingProxies();
      });

      // Added to connect React and API - previously CORS was blocking the connection.
      services.AddCors(opt =>
      {
        opt.AddPolicy("CorsPolicy", policy =>
              {
                // Here I allowed React app (localhost:3000) to use any headers, methods in API.
                policy
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .WithExposedHeaders("WWW-Authenticate")
                  .WithOrigins("http://localhost:3000")
                  .AllowCredentials();
              });
      });
      // Included the 'using Application.Activities' for List here.
      // I will have many handlers, but MediatR works in such a way that I only need to sepcify one.
      // MediatR will get reference of List.Handler to look inside its assembly and through that will get info about
      // all the others handlers.
      services.AddMediatR(typeof(List.Handler).Assembly);
      services.AddAutoMapper(typeof(List.Handler));
      services.AddSignalR();

      services.AddControllers(opt => 
      {
        var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
        opt.Filters.Add(new AuthorizeFilter(policy));
      })
        .AddFluentValidation(cfg => 
        {
          // No need to add other classes (delete, details...), one will work.
          cfg.RegisterValidatorsFromAssemblyContaining<Create>();
        });

      // These allows creating and managing users.
      var builder = services.AddIdentityCore<AppUser>();
      var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);
      identityBuilder.AddEntityFrameworkStores<DataContext>();
      identityBuilder.AddSignInManager<SignInManager<AppUser>>();

      // This adds a custom policy that I created for checking if
      // current user is a host.
      services.AddAuthorization(opt =>
      {
        opt.AddPolicy("IsActivityHost", policy =>
        {
          policy.Requirements.Add(new IsHostRequirement());
        });
      });
      // AddTransient makes it only avaiable for a lifetime of operation,
      // not the complete request.
      services.AddTransient<IAuthorizationHandler, IsHostRequirementHandler>();

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenKey"]));

      // That was required because program was throwing an error about ISystemClock.
      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(opt => 
        {
          opt.TokenValidationParameters = new TokenValidationParameters
          {
            // No request will be accepted w/o this signing key.
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
          };

          opt.Events = new JwtBearerEvents
          {
            OnMessageReceived = context =>
            {
              var accessToken = context.Request.Query["access_token"];
              var path = context.HttpContext.Request.Path;
              if (!string.IsNullOrEmpty(accessToken) &&
                (path.StartsWithSegments("/chat")))
              {
                context.Token = accessToken;
              }
              return Task.CompletedTask;
            }
          };
        });

      services.AddScoped<IJwtGenerator, JwtGenerator>();
      services.AddScoped<IUserAccessor, UserAccessor>();
      // Configuration is injected to Startup.cs at the start of the file.
      // this will get data from specified section - here, Cloudinary -
      // which is in user-secrets - we will explictly point to the user-secrets,
      // but it can be used to any json file.
      services.Configure<CloudinarySettings>(Configuration.GetSection("Cloudinary"));
      services.AddScoped<IPhotoAccessor, PhotoAccessor>();
      services.AddScoped<IProfileReader, ProfileReader>();
    }
    

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    // Here I can add middleware that is gonna do something w/ our request that gonna go through that pipeline.
    // 
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseMiddleware<ErrorHandlingMiddleware>();
      // Check if the app is being run in Dev Mode.
      if (env.IsDevelopment())
      {
        // app.UseDeveloperExceptionPage();
      }

      // Redirect HTTP requests to HTTPS requests. We will not use it in the beginning of the course.
      // app.UseHttpsRedirection();

      // typical files server would look for.
      app.UseDefaultFiles();

      app.UseStaticFiles();

      // This middleware allows the route resolution to happen earlier in the pipeline.
      // This also leads request from API to appropriate controller.
      app.UseRouting();

      // Added to resolve endpoints for React application.
      app.UseCors("CorsPolicy");

      app.UseAuthentication();
      // Used for determining user's identity, whether the user has access to a resource.
      app.UseAuthorization();



      // Map controllers endpoints to API - API server knows what to do when request comes and how to route it to appropriate controller.
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapHub<ChatHub>("/chat");
        endpoints.MapFallbackToController("Index", "Fallback");
      });
    }
  }
}
