using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using API.Common.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using API.Common.Services;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System;
using System.IO;
using API.Common.Interfaces;
using API.Common.Interfaces.Stripe;
using API.Common.Services.Stripe;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.Configure<AppSettings>(Configuration);

            var appSettings = Configuration.Get<AppSettings>();
            services.AddSingleton<HashingService>();
            services.AddScoped<TokenService>();
            services.AddSingleton<CryptographyService>();
            services.AddSingleton<SendGridEmailService>();
            services.AddSingleton<GenrateCodeSerivce>();
            services.AddSingleton<NotificationService>();
            services.AddScoped<IUserClaimService, UserClaimService>();
            services.AddScoped<IEnumValueService, EnumValueService>();
            services.AddScoped<ISubscriptionTierService, SubscriptionTierService>();
            services.AddScoped<ICompanyHierarchyService, CompanyHierarchyItemService>();
            services.AddScoped<IPaymentCouponService, PaymentCouponService>();
            services.AddScoped<IPaymentCustomerService, PaymentCustomerService>();
            services.AddScoped<IPaymentInvoiceService, PaymentInvoiceService>();
            services.AddScoped<IPaymentPlanService, PaymentPlanService>();
            services.AddScoped<IPaymentSubscriptionService, PaymentSubscriptionService>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IPlanSubscribeService, PlanSubscribeService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddAutoMapper(typeof(Startup));

            // configure jwt authentication
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.JwtSettings.Secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });
            // Config custom validation response
            services.AddMvc()
            .ConfigureApiBehaviorOptions(opt =>
            {
                opt.InvalidModelStateResponseFactory =
                   (context =>
                   {
                       string errorMsg = null;
                       var errors = context.ModelState.SelectMany(x => x.Value.Errors).Select(x => x.ErrorMessage ?? x.Exception?.Message).Where(x => x != null).ToList();
                       if (errors.Any())
                           errorMsg = string.Join("<br/>", errors);

                       return new BadRequestObjectResult(new ResponseJson<object>(null, errorMsg, HttpStatusCode.BadRequest, false));
                   }
                  );
            });
            services.AddControllers();
            services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
                c.DescribeAllEnumsAsStrings();

                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });

            });

            services.AddDbContext<APIContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("APIContext")));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));

            app.UseHttpsRedirection();

            app.UseRouting();
            //global exception
            app.UseExceptionHandler(builder => builder.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature.Error;

                var result = JsonConvert.SerializeObject(new ResponseJson<string>("", exception.Message, HttpStatusCode.BadRequest, false));
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }));

            app.UseCors(op => op.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
