using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application.BlockUser;

public sealed class CheckUserEndBanDate : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public CheckUserEndBanDate(IServiceScopeFactory serviceScopeFactory)
    {
        _scopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await unitOfWork.BlockedUsers.RemoveAllExpiredBanned();
            await unitOfWork.SaveAsync();
            await Task.Delay(3600000 , stoppingToken);
        }
    }
}

