using Enums;

namespace Entities;

internal class Ship
{
    public ShipType Type { get; set; }
    public bool IsDestroyed { get; set; } = false;
    public List<Cell> OccupiedCells { get; set; } = [];
}
