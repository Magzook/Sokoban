namespace Sokoban.Logic.WarehouseObjects.MainLayer.Cells;

public class EmptyCell : ICell
{
    private static readonly EmptyCell instance = new EmptyCell();

    private EmptyCell()
    {
    }

    public static EmptyCell Get() => instance;
}