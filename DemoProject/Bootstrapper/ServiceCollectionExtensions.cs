using AutoMapper;
using DemoProject.Bootstrapper.Behaviors;
using DemoProject.Bootstrapper.Helpers;
using DemoProject.Domain.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DemoProject.Bootstrapper
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureDemoProject(this IServiceCollection services, IConfiguration configuration)
        {
            return services.ConfigureMyApp(configuration, new List<Assembly>
            {
                Assembly.Load("DemoProject")

            }.ToArray());
        }

        public static IServiceCollection ConfigureMyApp(this IServiceCollection services, IConfiguration configuration, params Assembly[] assemblies)
        {
            services
                .ConfigureEntityFramework(configuration.GetConnectionString("DefaultConnection"))
                .ConfigureFluentValidaton(assemblies)
                .ConfigureMediatR(assemblies)
                .ConfigureAutoMapper(assemblies)
                .ConfigureAzureAuthProvider(assemblies);

            return services;
        }

        public static IServiceCollection ConfigureEntityFramework(this IServiceCollection services, string connectionString, params Assembly[] assemblies)
        {
            services.AddDbContext<DemoProjectDBContext>(options =>
            {
                options.UseSqlServer(connectionString);

                options.EnableSensitiveDataLogging(true);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
            });

            return services;
        }
        public static IServiceCollection ConfigureFluentValidaton(this IServiceCollection services, params Assembly[] assemblies)
        {
            foreach (var result in AssemblyScanner.FindValidatorsInAssemblies(assemblies))
                services.AddTransient(result.InterfaceType, result.ValidatorType);

            return services;
        }

        public static IServiceCollection ConfigureMediatR(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddMediatR(assemblies);

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

            return services;
        }

        public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddAutoMapper(assemblies);

            return services;
        }

        public static IServiceCollection ConfigureAzureAuthProvider(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddSingleton(typeof(IAzureAuthProvider), typeof(AzureAuthProvider));

            return services;
        }
    }
}
