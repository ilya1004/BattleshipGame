using Entities;

namespace Hubs;

public interface IGameListHub
{
    Task ReceiveActiveGames(List<Game> activeGames);
}
