using System;
using System.Collections.Generic;
using Sokoban.Logic.WarehouseObjects.AreaLayer;
using Sokoban.Logic.WarehouseObjects.AreaLayer.Areas;
using Sokoban.Logic.WarehouseObjects.MainLayer;
using Sokoban.Logic.WarehouseObjects.MainLayer.Cells;
using Sokoban.Logic.WarehouseObjects.PlayerLayer;
using Sokoban.Logic.WarehouseObjects.PlayerLayer.PlayableCharacters;

namespace Sokoban.Logic;

public class Warehouse
{
    public int BoxesOutOfPlaceCount { get; private set; }
    public readonly AreaMaster AreaMaster;
    public readonly MainLayerField MainLayerField;
    public readonly Dictionary<int, PlayableCharacter> DictIdToPlayer = new();

    public Warehouse(
        MainLayerField mainLayerField,
        AreaMaster areaMaster,
        List<PlayableCharacter> players)
    {
        this.MainLayerField = mainLayerField;
        this.AreaMaster = areaMaster;
        foreach (var player in players)
            DictIdToPlayer.Add(player.Id, player);
        
        // TODO: вычислить BoxesOutOfPlaceCount,
        // TODO: провалидировать areaMaster и игроков
        // TODO: (координаты не могут выходить за пределы mainLayerField);
        // TODO: проверить, что игроки полностью окружены стенами
    }

    /// <summary>
    /// Для нужд редактора
    /// </summary>
    public static Warehouse MakeFilledWithWalls(int width, int height)
    {
        var field = new MainLayerField(width, height);
        for (var x = 0; x < width; x++)
        for (var y = 0; y < height; y++)
        {
            field[x, y] = Wall.Get(); // TODO: лучше заменить на EmptyCell.Get(), но тогда надо рисовать сетку
        }
        var areaMaster = new AreaMaster();
        var players = new List<PlayableCharacter>();
        return new Warehouse(field, areaMaster, players);
    }
    
    public void HandlePlayerMovementIntention(int playableCharacterId, Direction direction)
    {
        // TODO: можно возвращать enum, соответствующий результату обработки,
        // TODO: тогда можно прикрутить audio для ходьбы, движения ящика и т. д.
        var playableCharacter = DictIdToPlayer.GetValueOrDefault(playableCharacterId);
        if (playableCharacter == null)
            throw new Exception($"Player with id {playableCharacterId} doesn't exist in the warehouse");

        if (playableCharacter is not Storekeeper)
            return;
        
        var storekeeper = (Storekeeper) playableCharacter;
        
        var (deltaX, deltaY) = direction switch
        {
            Direction.Up => (0, -1),
            Direction.Down => (0, 1),
            Direction.Left => (-1, 0),
            Direction.Right => (1, 0),
            _ => (0, 0)
        };
        if (deltaX == 0 && deltaY == 0)
            return;
        
        var targetX = storekeeper.X + deltaX;
        var targetY = storekeeper.Y + deltaY;
        var targetCell = MainLayerField[targetX, targetY];
        switch (targetCell)
        {
            case EmptyCell:
                storekeeper.X = targetX;
                storekeeper.Y = targetY;
                break;
            
            case Box box:
                var xBehindBox = targetX + (targetX - storekeeper.X);
                var yBehindBox = targetY + (targetY - storekeeper.Y);
                var cellBehindBox = MainLayerField[xBehindBox, yBehindBox];
                if (cellBehindBox is EmptyCell)
                {
                    MainLayerField[targetX, targetY] = EmptyCell.Get();
                    MainLayerField[xBehindBox, yBehindBox] = box;
                    storekeeper.X = targetX;
                    storekeeper.Y = targetY;
                    
                    var targetArea = AreaMaster.TryGetAreaAt(targetX, targetY);
                    var behindBoxArea = AreaMaster.TryGetAreaAt(xBehindBox, yBehindBox);

                    if (targetArea is null && behindBoxArea is BoxArea)
                    {
                        BoxesOutOfPlaceCount--;
                    }
                    else if (targetArea is BoxArea && behindBoxArea is null)
                    {
                        BoxesOutOfPlaceCount++;
                    }
                }
                break;
        }
    }
}