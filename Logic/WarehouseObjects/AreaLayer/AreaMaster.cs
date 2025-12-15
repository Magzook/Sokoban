using System.Collections.Generic;

namespace Sokoban.Logic.WarehouseObjects.AreaLayer;

public class AreaMaster
{
    private readonly Dictionary<(int x, int y), IArea> dictCoordsToArea = new();
    
    public void SetAreaAt(int x, int y, IArea area)
        => dictCoordsToArea[(x, y)] = area;

    public IArea TryGetAreaAt(int x, int y)
        => dictCoordsToArea.GetValueOrDefault((x, y));

    public IEnumerable<KeyValuePair<(int x, int y), IArea>> AllAreasWithCoords()
    {
        foreach (var pair in dictCoordsToArea)
            yield return pair;
    }
}