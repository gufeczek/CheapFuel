﻿using System.Linq;
using Application.Common.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Pipeline.Operations.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI.IntegrationTests.TestConfiguration;

public class TestingWebApiFactory<TEntity> : WebApplicationFactory<Program> where TEntity : class
{

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(AppDbContext));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<AppDbContext, InMemoryDbContext>();

            services.AddScoped<IEmailSenderService, EmailSenderServiceTest>();
            services.AddScoped<IRemovalHandlingOperation, RemovalHandlingOperationTest>();
        });
    }
}