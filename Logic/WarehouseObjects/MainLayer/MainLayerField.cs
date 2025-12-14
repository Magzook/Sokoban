namespace Sokoban.Logic.WarehouseObjects.MainLayer;

public class MainLayerField
{
    private readonly ICell[,] twoDimArray;

    public MainLayerField(int width, int height)
    {
        twoDimArray = new ICell[height, width];
    }
    
    public int Height => twoDimArray.GetLength(0);

    public int Width => twoDimArray.GetLength(1);
    
    public int MaxX => Width - 1;
    
    public int MaxY => Height - 1;
    
    public int MinX => 0;
    
    public int MinY => 0;
    
    public ICell this[int x, int y]
    {
        get => twoDimArray[y, x];
        set => twoDimArray[y, x] = value;
    }
}