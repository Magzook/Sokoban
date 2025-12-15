namespace Sokoban.Logic;

public class Level
{
    public readonly string Name;
    public readonly Warehouse Warehouse;
    // TODO: добавить больше полей (таймер, количество сделанных кодов и пр.)
    
    public Level(string name, Warehouse warehouse)
    {
        this.Name = name;
        this.Warehouse = warehouse;
    }

    public void OnPressDirection(int playableCharacterId, Direction direction)
        => Warehouse.HandlePlayerMovementIntention(playableCharacterId, direction);
}