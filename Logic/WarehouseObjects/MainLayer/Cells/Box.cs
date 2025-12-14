namespace Sokoban.Logic.WarehouseObjects.MainLayer.Cells;

public class Box : ICell
{
    private static readonly Box instance = new Box();

    private Box()
    {
    }
    
    public static Box Get() => instance;
}