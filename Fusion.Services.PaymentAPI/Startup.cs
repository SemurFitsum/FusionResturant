﻿using AzureMessageBus;
using Fusion.Services.PaymentAPI.Messaging;
using Mango.Services.PaymentAPI.Extension;
using Microsoft.OpenApi.Models;
using PaymentProcessor;

namespace Fusion.Services.PaymentAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddDbContext<ApplicationDbContext>(options =>
            //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
            //services.AddSingleton(mapper);
            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //services.AddScoped<IOrderRepository, OrderRepository>();

            //var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            //optionBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));

            //services.AddSingleton(new OrderRepository(optionBuilder.Options));

            //services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();

            services.AddSingleton<IProcessPayment, ProcessPayment>();

            services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();

            services.AddSingleton<IMessageBus, AzureServiceBusMessageBus>();

            services.AddControllers();

            //services.AddAuthentication("Bearer")
            //    //.AddJwtBearer("Bearer", options =>
            //    {
            //        options.Authority = "https://localhost:7238/";
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateAudience = false
            //        };
            //    });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "fusion");
                });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fusion.Services.PaymentAPI", Version = "v1" });
                //c.EnableAnnotations();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"Enter 'Bearer' [space] and your token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },

                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },

                        new List<string>()
                    }
                });


            });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseAzureServiceBusConsumer();
        }
    }
}
