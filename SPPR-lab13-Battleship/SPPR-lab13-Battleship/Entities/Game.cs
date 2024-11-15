namespace Entities;

internal class Game
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsPlaying { get; set; } = false;
    public int? PlayerId1 { get; set; }
    public string PlayerUsername1 { get; set; } = string.Empty;
    public int? PlayerId2 { get; set; }
    public string PlayerUsername2 { get; set; } = string.Empty;
    public int Winner { get; set; } = 0;
    public GameState? GameState { get; set; } = null;
}