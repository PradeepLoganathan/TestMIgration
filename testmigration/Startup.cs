using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using testmigration.Data;
using testmigration.Models;
using testmigration.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.OAuth;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;

//using Microsoft.Owin.Security;
namespace testmigration
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(15);
            });
            services.AddIdentity<ApplicationUser, ApplicationRole>(options => {

                options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromMinutes(15);

            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

          

            services.AddMvc();
            services.AddSingleton<IDistributedCache>(
                serviceProvider =>
                new RedisCache(
                    new RedisCacheOptions
                    {
                        Configuration = "localhost",
                        InstanceName = "testMigration"

                    }
                ));

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();
            //app.UseIdentity().UseCookieAuthentication(
            //            new CookieAuthenticationOptions
            //            {
            //                ExpireTimeSpan = TimeSpan.FromMinutes(3)
            //            }
            //            );
            app.UseOAuthAuthentication(GitHubOptions);
            app.UseOAuthAuthentication(PinterestOptions);
            app.UseFacebookAuthentication(new FacebookOptions()
            {
                AppId = Configuration["Authentication:Facebook:AppId"],
                AppSecret = Configuration["Authentication:Facebook:AppSecret"]
            });
            app.UseTwitterAuthentication(new TwitterOptions()
            {
                ConsumerKey = Configuration["Authentication:Twitter:ConsumerKey"],
                ConsumerSecret = Configuration["Authentication:Twitter:ConsumerSecret"]
            });
            app.UseGoogleAuthentication(new GoogleOptions()
            {
                ClientId = Configuration["Authentication:Google:ClientId"],
                ClientSecret = Configuration["Authentication:Google:ClientSecret"]
            });
            app.UseMicrosoftAccountAuthentication(new MicrosoftAccountOptions()
            {
                ClientId = Configuration["Authentication:Microsoft:ApplicationId"],
                ClientSecret = Configuration["Authentication:Microsoft:Password"]
            });



            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
                //template: "{controller=Account}/{action=TestPage}");
                //routes.MapRoute(
                //name: "signin-facebook",
                //template: "{controller=Account}/{action=ExternalLoginCallback}");
            });


        }
        private OAuthOptions GitHubOptions => new OAuthOptions
        {
            AuthenticationScheme = "GitHub",
            DisplayName = "GitHub",
            ClientId = Configuration["GitHub:ClientId"],
            ClientSecret = Configuration["GitHub:ClientSecret"],
            CallbackPath = new PathString("/signin-github"),
            AuthorizationEndpoint = "https://github.com/login/oauth/authorize",
            TokenEndpoint = "https://github.com/login/oauth/access_token",
            UserInformationEndpoint = "https://api.github.com/user",
            ClaimsIssuer = "OAuth2-Github",
            //SaveTokensAsClaims = true,

            // Retrieving user information is unique to each provider.
            Events = new OAuthEvents {
                OnCreatingTicket = async context => { await CreateGitHubAuthTicket(context); }
            }
        };
        private OAuthOptions PinterestOptions => new OAuthOptions
        {
            AuthenticationScheme = "Pinterest",
            DisplayName = "Pinterest",
            ClientId = Configuration["Pinterest:AppId"],
            ClientSecret = Configuration["Pinterest:AppSecret"],
            CallbackPath = new PathString("/signin-Pinterest"),
            AuthorizationEndpoint = "https://api.pinterest.com/oauth/",
            TokenEndpoint = "https://api.pinterest.com/v1/oauth/token ",
            UserInformationEndpoint = "https://api.pinterest.com/v1/me/pins",
            ClaimsIssuer = "OAuth2-Pinterest",
            //SaveTokensAsClaims = true,

            // Retrieving user information is unique to each provider.
            Events = new OAuthEvents
            {
                OnCreatingTicket = async context => { await CreatePinterestAuthTicket(context); }
            }
        };
        private static async Task CreatePinterestAuthTicket(OAuthCreatingTicketContext context)
        {
            // Get the Pinterest user
            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
            response.EnsureSuccessStatusCode();

            var user = JObject.Parse(await response.Content.ReadAsStringAsync());

            AddClaims(context, user);
        }
        private static async Task CreateGitHubAuthTicket(OAuthCreatingTicketContext context)
        {
            // Get the GitHub user
            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
            response.EnsureSuccessStatusCode();

            var user = JObject.Parse(await response.Content.ReadAsStringAsync());

            AddClaims(context, user);
        }
        private static void AddClaims(OAuthCreatingTicketContext context, JObject user)
        {
            var identifier = user.Value<string>("id");
            if (!string.IsNullOrEmpty(identifier))
            {
                context.Identity.AddClaim(new Claim(
                    ClaimTypes.NameIdentifier, identifier,
                    ClaimValueTypes.String, context.Options.ClaimsIssuer));
            }
            
            var userData = user.ToString();
            if (!string.IsNullOrEmpty(userData))
            {
                context.Identity.AddClaim(new Claim(
                    ClaimTypes.UserData, userData,
                    ClaimValueTypes.String, context.Options.ClaimsIssuer));
            }
            var userName = user.Value<string>("login");
            if (!string.IsNullOrEmpty(userName))
            {
                context.Identity.AddClaim(new Claim(
                    ClaimsIdentity.DefaultNameClaimType, userName,
                    ClaimValueTypes.String, context.Options.ClaimsIssuer));
            }

            var name = user.Value<string>("name");
            if (!string.IsNullOrEmpty(name))
            {
                context.Identity.AddClaim(new Claim(
                    ClaimTypes.Name, name,
                    ClaimValueTypes.String, context.Options.ClaimsIssuer));
            }

            var link = user.Value<string>("url");
            if (!string.IsNullOrEmpty(link))
            {
                context.Identity.AddClaim(new Claim(
                    "urn:github:url", link,
                    ClaimValueTypes.String, context.Options.ClaimsIssuer));
            }
            var DateOfBirth = user.Value<string>("DateOfBirth");
            if (!string.IsNullOrEmpty(DateOfBirth))
            {
                context.Identity.AddClaim(new Claim(
                    "urn:github:DateOfBirth", DateOfBirth,
                    ClaimValueTypes.String, context.Options.ClaimsIssuer));
            }
            
            var gender = user.Value<string>("gender");
            if (!string.IsNullOrEmpty(gender))
            {
                context.Identity.AddClaim(new Claim(
                    "urn:github:gender", gender,
                    ClaimValueTypes.String, context.Options.ClaimsIssuer));
            }
        }
    }
}
