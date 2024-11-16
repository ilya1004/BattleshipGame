namespace Entities;

public class Cell
{
    public int CoordX { get; set; }
    public int CoordY { get; set; }
    public bool IsHited { get; set; } = false;
    public bool HasShip { get; set; } = false;

    public Cell(int coordX, int coordY)
    {
        CoordX = coordX;
        CoordY = coordY;
    }
}
