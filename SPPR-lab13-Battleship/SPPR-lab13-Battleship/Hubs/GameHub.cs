using Entities;
using Enums;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;

namespace Hubs;

public class GameHub : Hub<IGameHub>
{
    private readonly IDistributedCache _distributedCache;
    public GameHub(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task PlaceShipOnBoard(int gameId, Ship ship)
    {

    }
}
