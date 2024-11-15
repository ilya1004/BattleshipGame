namespace Entities;

internal class GameState
{
    public Board Player1Board { get; set; } = new Board();
    public Board Player2Board { get; set; } = new Board();
    public int CurrentTurn { get; set; } // 1 для Player1, 2 для Player2
    public bool IsGameOver { get; set; } = false;

}
