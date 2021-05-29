using System.Text;
using System.Threading.Tasks;
using CourseProject.Api.Comments;
using CourseProject.Api.Services;
using CourseProject.Api.Services.Abstraction;
using CourseProject.Data;
using CourseProject.Data.Abstract;
using CourseProject.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace CourseProject.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            { 
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000", "https://courseproject.vercel.app", "https://webapp-210513212326.azurewebsites.net")
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .SetIsOriginAllowed(_ => true);
                    });
            });

            services.AddControllers();
            services
                .AddDbContext<CourseProjectContext>(options =>
                    options.UseSqlServer(
                        Configuration.GetConnectionString("CourseProjectContext"),
                        o => o.MigrationsAssembly("CourseProject.Api")
                    )
                );

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration.GetValue<string>("JWTSecretKey"))
                        )
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/comments")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            
            
            services.AddScoped<ICommentsRepository, CommentsRepository>();
            services.AddScoped<ICompanyBenefitRepository, CompanyBenefitRepository>();
            services.AddScoped<ICompanyNewsRepository, CompanyNewsRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICompanyRatingRepository, CompanyRatingRepository>();
            services.AddScoped<ICrowdfundingCompanyRepository, CrowdfundingCompanyRepository>();
            services.AddScoped<ILikesOrDislikesRepository, LikesOrDislikesRepository>();
            services.AddScoped<IPaymentsRepository, PaymentsRepository>();
            services.AddScoped<IPhotosRepository, PhotosRepository>();
            services.AddScoped<IUserBenefitRepository, UserBenefitRepository>();
            services.AddScoped<IVideosRepository, VideosRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRolesRepository, RolesRepository>();
            services.AddScoped<ICompanyTagsRepository, CompanyTagsRepository>();
            services.AddScoped<ITagsRepository, TagsRepository>();

            services.AddSignalR();

            services.AddTransient<ITokenService, TokenService>();
            services
                .AddMvc(options => options.EnableEndpointRouting = true)
                .SetCompatibilityVersion(CompatibilityVersion.Latest)
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseAuthentication();
            app.UseCors();
            app.UseAuthorization();
            app.UseHttpsRedirection();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<CommentsHub>("/comments");
                endpoints.MapControllers();
            });
        }
    }
}