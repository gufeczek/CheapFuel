using System;
using System.Net.Http;
using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.IntegrationTests.PredefinedData;
using WebAPI.IntegrationTests.TestConfiguration;
using Xunit;
using Xunit.Abstractions;

namespace WebAPI.IntegrationTests;

[Collection("Sequential")]
public abstract class IntegrationTest : IClassFixture<TestingWebApiFactory<Program>>, IDisposable
{
    private readonly IPredefinedData[] _predefinedData;
    protected ITestOutputHelper OutputHelper;
    
    public TestingWebApiFactory<Program> Factory { get; }
    public HttpClient HttpClient { get; }
    
    protected IntegrationTest(TestingWebApiFactory<Program> factory, ITestOutputHelper outputHelper, IPredefinedData[] predefinedData)
    {
        _predefinedData = predefinedData;
        Factory = factory;
        OutputHelper = outputHelper;

        InitDatabaseWithData();
        
        HttpClient = factory.CreateClient();
    }
    
    private void InitDatabaseWithData()
    {
        var sp = Factory.Services.GetService<IServiceScopeFactory>();
        using var scope = sp.CreateScope();
        var context = scope.ServiceProvider.GetService<AppDbContext>();
        
        foreach (var predefinedData in _predefinedData)
        {
            predefinedData.Seed(context!);
        }
    }

    public void Dispose()
    {
        var sp = Factory.Services.GetService<IServiceScopeFactory>();
        using var scope = sp.CreateScope();
        var context = scope.ServiceProvider.GetService<AppDbContext>();

        for (var i = _predefinedData.Length - 1; i >= 0; i--)
        {
            _predefinedData[i].Clear(context!);
        }
    }
}
