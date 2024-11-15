namespace Entities;

internal class Board
{
    public Cell[,] Cells { get; set; } = new Cell[10, 10]; // 10x10 поле
    public List<Ship> Ships { get; set; } = [];

    public Board()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Cells[i, j] = new Cell(j, i);
            }
        }
    }
}