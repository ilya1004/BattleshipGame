using Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Hubs;

public class GamesListHub : Hub<IGameListHub>
{
    private readonly IDistributedCache _distributedCache;
    private const string ActiveGameIdsInCacheKey = "ActiveGameIds";
    private const string PlayingGameIdsInCacheKey = "PlayingGameIds";
    private const string ActiveUserIdsInCacheKey = "activeUserIds";
    private const string NextGameIdKey = "nextGameId";
    public List<int> ActiveGameIds { get; set; } = [];
    public List<int> PlayingGameIds { get; set; } = [];
    public List<Game> ActiveGames { get; set; } = [];

    public GamesListHub(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public override Task OnConnectedAsync()
    {
        string connectionId = Context.ConnectionId;
        Console.WriteLine($"Client connected with ID: {connectionId}");

        if (_distributedCache.GetString(ActiveGameIdsInCacheKey) == null)
        {
            _distributedCache.SetString(ActiveGameIdsInCacheKey, JsonConvert.SerializeObject(ActiveGameIds));
        }

        if (_distributedCache.GetString(PlayingGameIdsInCacheKey) == null)
        {
            _distributedCache.SetString(PlayingGameIdsInCacheKey, JsonConvert.SerializeObject(PlayingGameIds));
        }

        return base.OnConnectedAsync();
    }

    public async Task CreateGame(int userId, string UserName, string gameName)
    {
        var game = new Game
        {
            Id = 0,
            Name = gameName,
            IsPlaying = false,
            PlayerId1 = userId,
            PlayerUsername1 = UserName,
            PlayerId2 = null,
            PlayerUsername2 = string.Empty,
            Winner = 0,
            GameState = null,
        };

        //List<int> activeUserIds = await GetActiveUserIdsFromCache();

        //if (activeUserIds.Contains(userId))
        //{
        //    return -1;
        //}

        await UpdateActiveGameIdsFromCache();

        string? cachedLastGameId = await _distributedCache.GetStringAsync(NextGameIdKey);

        if (string.IsNullOrEmpty(cachedLastGameId))
        {
            game.Id = 1;
            await _distributedCache.SetStringAsync(NextGameIdKey, "1");
        }

        game.Id = Convert.ToInt32(cachedLastGameId);

        ActiveGameIds.Add(game.Id);

        await SetGameIdsToCache();

        await _distributedCache.SetStringAsync($"game-{game.Id}", JsonConvert.SerializeObject(game));
        await _distributedCache.SetStringAsync(NextGameIdKey, (game.Id + 1).ToString());
    }

    public async Task RemoveGame(int gameId)
    {
        await UpdateActiveGamesFromCache();

        await UpdateActiveGameIdsFromCache();

        if (ActiveGameIds.Remove(gameId))
        {
            await SetGameIdsToCache();
        }

        var gameToRemove = ActiveGames.FirstOrDefault(game => game.Id == gameId);
        
        if (gameToRemove != null)
        {
            await _distributedCache.RemoveAsync($"game-{gameToRemove.Id}");
        }
    }

    public async Task StartGame(int gameId, int userId, string username)
    {
        await UpdatePlayingGameIdsFromCache();
        await UpdateActiveGamesFromCache();

        var game = ActiveGames.FirstOrDefault(game => game.Id == gameId);

        if (game != null)
        {
            game.IsPlaying = true;
            game.PlayerId2 = userId;
            game.PlayerUsername2 = username;
            game.GameState = new GameState();

            await _distributedCache.SetStringAsync($"game-{game.Id}", JsonConvert.SerializeObject(game));

            PlayingGameIds.Add(gameId);
            await SetPlayingGameIdsToCache();
        }
    }

    public async Task GetActiveGames()
    {
        await UpdateActiveGamesFromCache();
        await Clients.All.ReceiveActiveGames(ActiveGames);
    }

    private async Task<List<int>> GetActiveUserIdsFromCache()
    {
        string? keys = await _distributedCache.GetStringAsync(ActiveUserIdsInCacheKey);
        if (string.IsNullOrEmpty(keys))
        {
            return [];
        }

        var activeUserIds = JsonConvert.DeserializeObject<List<int>>(keys);

        if (activeUserIds is null)
        {
            return [];
        }

        return activeUserIds;
    }

    private async Task SetGameIdsToCache()
    {
        await _distributedCache.SetStringAsync(ActiveGameIdsInCacheKey, JsonConvert.SerializeObject(ActiveGameIds));
    }

    private async Task UpdateActiveGameIdsFromCache()
    {
        string? keys = await _distributedCache.GetStringAsync(ActiveGameIdsInCacheKey);
        if (string.IsNullOrEmpty(keys))
        {
            return;
        }

        var activeGameIds = JsonConvert.DeserializeObject<List<int>>(keys);

        if (activeGameIds is null)
        {
            return;
        }

        ActiveGameIds = activeGameIds;
    }

    private async Task UpdateActiveGamesFromCache()
    {
        await UpdateActiveGameIdsFromCache();

        List<Game> activeGames = [];

        foreach (var id in ActiveGameIds)
        {
            string? cachedGame = await _distributedCache.GetStringAsync($"game-{id}");

            if (!string.IsNullOrEmpty(cachedGame))
            {
                activeGames.Add(JsonConvert.DeserializeObject<Game>(cachedGame)!);
            }
        }

        ActiveGames = activeGames;
    }

    private async Task SetPlayingGameIdsToCache()
    {
        await _distributedCache.SetStringAsync(PlayingGameIdsInCacheKey, JsonConvert.SerializeObject(PlayingGameIds));
    }

    private async Task UpdatePlayingGameIdsFromCache()
    {
        string? keys = await _distributedCache.GetStringAsync(PlayingGameIdsInCacheKey);
        if (string.IsNullOrEmpty(keys))
        {
            return;
        }

        var playingGameIds = JsonConvert.DeserializeObject<List<int>>(keys);

        if (playingGameIds is null)
        {
            return;
        }

        PlayingGameIds = playingGameIds;
    }
}
