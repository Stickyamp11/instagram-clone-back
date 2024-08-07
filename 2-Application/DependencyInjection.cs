using Domain.Entities;
using FluentResults;
using Instagram_Api.Application.Auth.Commands;
using Instagram_Api.Application.Auth.Models;
using Instagram_Api.Application.Common;
using Instagram_Api.Application.Publications.Commands;
using Instagram_Api.Application.Publications.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            AddCommands(services);

            AddQueries(services);

            return services;
        }

        private static IServiceCollection AddCommands(this IServiceCollection services)
        {
            services.AddScoped<IPipelineBehavior<RegisterCommand, Result<AuthResult>>,
                               ValidateRegisterCommandBehavior>();
            services.AddScoped<IPipelineBehavior<LoginCommand, Result<AuthResult>>,
                               ValidateLoginCommandBehavior>();

            services.AddScoped<IPipelineBehavior<AddPublicationCommand, Result<int>>,
                               ValidateAddPublicationCommandBehavior>();
            return services;
        }

        private static IServiceCollection AddQueries(this IServiceCollection services)
        {
            services.AddScoped<IPipelineBehavior<PublicationsByUserGuidQuery, Result<List<Publication>>>,
                               ValidatePublicationsByUserGuidQueryBehavior>();
            return services;
        }
    }
}
