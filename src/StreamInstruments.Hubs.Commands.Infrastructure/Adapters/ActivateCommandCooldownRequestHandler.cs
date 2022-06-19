using MediatR;
using StreamInstruments.Hubs.Commands.Infrastructure.Helpers;
using StreamInstruments.Hubs.Commands.SecondaryPorts.ActivateCommandCooldown;
using StreamInstruments.Interfaces;

namespace StreamInstruments.Hubs.Commands.Infrastructure.Adapters;

public class ActivateCommandCooldownRequestHandler : IRequestHandler<ActivateCommandCooldownRequest>
{
    private readonly ICacheService _cacheService;

    public ActivateCommandCooldownRequestHandler(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task<Unit> Handle(ActivateCommandCooldownRequest request, CancellationToken cancellationToken)
    {
        var cooldownCacheKey = CooldownKeyHelper.ConstructCooldownKey(request.CommandId, request.CooldownIdentifier);
        var cooldownCacheExpiry = TimeSpan.FromSeconds(request.CooldownSeconds);
        await _cacheService.WriteValueAsync(cooldownCacheKey, true, cooldownCacheExpiry, true, cancellationToken);

        return Unit.Value;
    }
}