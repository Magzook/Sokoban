using System.Text;
using System;
using System.Collections.Generic;
using Sokoban.Logic;
using Sokoban.Logic.WarehouseObjects.AreaLayer;
using Sokoban.Logic.WarehouseObjects.AreaLayer.Areas;
using Sokoban.Logic.WarehouseObjects.MainLayer;
using Sokoban.Logic.WarehouseObjects.MainLayer.Cells;
using Sokoban.Logic.WarehouseObjects.PlayerLayer;
using Sokoban.Logic.WarehouseObjects.PlayerLayer.PlayableCharacters;

namespace Sokoban.Serialization;

// TODO: добавить обработку всевозможных исключений, валидацию
public static class Mapper
{
    private const char CharEmpty = '_';
    private const char CharWall = 'W';
    private const char CharBox = 'B';
    
    public static Level LevelFromDataObject(XmlObjects.Level dataLevel)
    {
        var mainLayerField = MainLayerFieldFromDataObject(dataLevel.Warehouse.MainLayerField);
        var areaMaster = AreaMasterFromDataObject(dataLevel.Warehouse.AreaEntries);
        var playableCharacters = PlayableCharactersFromDataObject(dataLevel.Warehouse.PlayableCharacters);
        
        return new Level(
            dataLevel.Name,
            new Warehouse(
                mainLayerField, areaMaster, playableCharacters)
        );
    }

    public static XmlObjects.Level LevelToDataObject(Level level)
    {
        var dataMainLayerField = MainLayerFieldToDataObject(level.Warehouse.MainLayerField);

        var dataAreaEntries = AreaMasterToDataObject(level.Warehouse.AreaMaster);
        
        var dataPlayableCharacters = PlayableCharactersToDataObject(level.Warehouse.DictIdToPlayer);
        
        return new XmlObjects.Level
        {
            Name = level.Name,
            Warehouse = new XmlObjects.Warehouse
            {
                MainLayerField = dataMainLayerField,
                AreaEntries = dataAreaEntries,
                PlayableCharacters = dataPlayableCharacters
            }
        };
    }

    private static MainLayerField MainLayerFieldFromDataObject(XmlObjects.MainLayerField dataMainLayerField)
    {
        var rows = dataMainLayerField.FieldData.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var fieldHeight = rows.Length;
        var fieldWidth = rows[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
        var field = new MainLayerField(width: fieldWidth, height: fieldHeight);
        
        for (var y = 0; y < fieldHeight; y++)
        {
            var rowCellsAsStrings = rows[y].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (var x = 0; x < fieldWidth; x++)
            {
                var cellAsChar = rowCellsAsStrings[x][0];
                field[x, y] = cellAsChar switch
                {
                    CharWall => Wall.Get(),
                    CharEmpty => EmptyCell.Get(),
                    CharBox => Box.Get(),
                    _ => throw new Exception($"Unknown character: {cellAsChar} at row {y} column {x}")
                };
            }
        }
        
        return field;
    }

    private static XmlObjects.MainLayerField MainLayerFieldToDataObject(MainLayerField mainLayerField)
    {
        var builder = new StringBuilder();
        builder.AppendLine();
        for (var y = 0; y < mainLayerField.Height; y++)
        {
            for (var x = 0; x < mainLayerField.Width; x++)
            {
                var cell = mainLayerField[x, y];
                var cellAsChar = cell switch
                {
                    Wall => CharWall,
                    EmptyCell => CharEmpty,
                    Box => CharBox,
                    _ => throw new Exception($"Don't know how to serialize cell {cell}")
                };
                builder.Append(cellAsChar).Append(' ');
            }
            builder.AppendLine();
        }
        return new XmlObjects.MainLayerField { FieldData = builder.ToString() };
    }

    private static AreaMaster AreaMasterFromDataObject(List<XmlObjects.AreaEntry> dataAreaEntries)
    {
        var areaMaster = new AreaMaster();

        foreach (var dataAreaEntry in dataAreaEntries)
        {
            var className = dataAreaEntry.Area.Class;
            IArea area;
            if (className.Equals("BoxArea"))
                area = BoxArea.Get();
            else
                throw new Exception($"Unsupported area type: {className}");
            areaMaster.SetAreaAt(dataAreaEntry.X, dataAreaEntry.Y, area);
        }
        
        return areaMaster;
    }

    private static List<XmlObjects.AreaEntry> AreaMasterToDataObject(AreaMaster areaMaster)
    {
        var dataAreaEntries = new List<XmlObjects.AreaEntry>();
        foreach (var pair in areaMaster.AllAreasWithCoords())
        {
            var dataAreaEntry = new XmlObjects.AreaEntry
            {
                X = pair.Key.x,
                Y = pair.Key.y,
                Area = new XmlObjects.Area { Class = pair.Value.GetType().Name }
            };
            dataAreaEntries.Add(dataAreaEntry);
        }

        return dataAreaEntries;
    }

    private static List<PlayableCharacter> PlayableCharactersFromDataObject(
        List<XmlObjects.PlayableCharacter> dataPlayableCharacters)
    {
        var playableCharacters = new List<PlayableCharacter>();

        foreach (var dataPlayableCharacter in dataPlayableCharacters)
        {
            var className = dataPlayableCharacter.Class;
            PlayableCharacter playableCharacter;
            if (className.Equals("Storekeeper"))
                playableCharacter = new Storekeeper(dataPlayableCharacter.Id, dataPlayableCharacter.X, dataPlayableCharacter.Y);
            else
                throw new Exception($"Unsupported playable character type: {className}");
            
            playableCharacters.Add(playableCharacter);
        }

        return playableCharacters;
    }

    private static List<XmlObjects.PlayableCharacter> PlayableCharactersToDataObject(
        Dictionary<int, PlayableCharacter> dictIdToPlayer
    )
    {
        var dataPlayableCharacters = new List<XmlObjects.PlayableCharacter>();
        foreach (var playableCharacter in dictIdToPlayer.Values)
        {
            var dataPlayableCharacter = new XmlObjects.PlayableCharacter
            {
                Id = playableCharacter.Id,
                X = playableCharacter.X,
                Y = playableCharacter.Y,
                Class = playableCharacter.GetType().Name
            };
            dataPlayableCharacters.Add(dataPlayableCharacter);
        }

        return dataPlayableCharacters;
    }
}