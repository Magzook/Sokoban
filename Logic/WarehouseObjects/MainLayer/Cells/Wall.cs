namespace Sokoban.Logic.WarehouseObjects.MainLayer.Cells;

public class Wall : ICell
{
    private static readonly Wall instance = new();

    private Wall()
    {
    }

    public static Wall Get() => instance;
}