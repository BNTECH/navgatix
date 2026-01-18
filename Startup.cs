using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using satguruApp.DLL.Models;
using satguruApp.Service;
using satguruApp.Service.Services;
using satguruApp.Service.Services.Interfaces;
using satguruApp.Service.ViewModels;
using Swashbuckle.AspNetCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Claims;


namespace navgatix
{
    public class Startup
    {
        private IConfiguration Configuration;
        private IWebHostEnvironment _env;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SatguruDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<SatguruDBContext>(options =>
                   options.UseSqlServer(
                       Configuration.GetConnectionString("DefaultConnection"),
                       b =>
                       {
                           b.MigrationsAssembly(typeof(SatguruDBContext).Assembly.FullName); //ApplicationDbContext
                       }));

            services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<SatguruDBContext>().AddDefaultTokenProviders();
            services.Configure<JWT>(Configuration.GetSection("JWT"));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserInfoService, UserInfoService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IAccountTypeService, AccountTypeService>();
            services.AddScoped<IAppCustormer, AppCustomer>();
            services.AddScoped<IBookingService, BookingService>();
            //services.AddScoped<IRepository, Repository>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ICommonTypeService, CommonTypeService>();
            services.AddScoped<ITransportService, TransportService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IStateService, StateService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<ISystemConfigurationService, SystemConfigurationService>();


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            .AddJwtBearer(o =>
            {
                o.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async ctx =>
                    {
                        Configuration.Bind("AzureAd", o);
                        o.TokenValidationParameters.NameClaimType = "name";
                        string userId = ctx.Principal.FindFirstValue("name");

                        //Get EF context
                        var _db = ctx.HttpContext.RequestServices.GetRequiredService<SatguruDBContext>();

                        //Check if this app can read confidential items
                        var user = await _db.Users.FirstOrDefaultAsync(a => a.UserName == userId || a.Email == userId);
                        var claims = new List<Claim>();
                        claims.Add(new Claim("UserName", user?.UserName.ToString() ?? ""));
                        var appIdentity = new ClaimsIdentity(claims);
                        ctx.Principal.AddIdentity(appIdentity);
                    }
                };

                o.RequireHttpsMetadata = false;
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };

            });
            services.AddCors();
            services.AddControllers();
            services.AddControllersWithViews();
            services.AddEndpointsApiExplorer();
            // ✅ Add Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            AppDomain.CurrentDomain.FirstChanceException += (sender, e) =>
            {
                if (e.Exception is ReflectionTypeLoadException rtlEx)
                {
                    foreach (var loaderException in rtlEx.LoaderExceptions)
                    {
                        Console.WriteLine(loaderException.Message);
                    }
                }
            };

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = "swagger";
            });
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();



            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSwagger();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "clientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });

        }
    }
}
