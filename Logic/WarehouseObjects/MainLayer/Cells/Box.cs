namespace Sokoban.Logic.WarehouseObjects.MainLayer.Cells;

public class Box : ICell
{
    private static readonly Box instance = new();

    private Box()
    {
    }
    
    public static Box Get() => instance;
}