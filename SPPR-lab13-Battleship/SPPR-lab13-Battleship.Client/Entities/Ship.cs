using ClientEnums;

namespace Entities;

public class Ship
{
    public ShipType Type { get; set; }
    public bool IsDestroyed { get; set; } = false;
    public List<Cell> OccupiedCells { get; set; } = [];
}
