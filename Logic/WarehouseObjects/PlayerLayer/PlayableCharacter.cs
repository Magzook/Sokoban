namespace Sokoban.Logic.WarehouseObjects.PlayerLayer;

public abstract class PlayableCharacter
{
    public readonly int Id;
    public int X { get; set; }
    public int Y { get; set; }

    protected PlayableCharacter(int id, int x, int y)
    {
        Id = id;
        X = x;
        Y = y;
    }
}