using Sokoban.Library;

namespace Sokoban.UI;

using Microsoft.Xna.Framework;
using Sokoban.Logic;
using Sokoban.Logic.WarehouseObjects.AreaLayer.Areas;
using Sokoban.Logic.WarehouseObjects.MainLayer.Cells;
using Sokoban.Logic.WarehouseObjects.PlayerLayer.PlayableCharacters;

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

public class WarehouseDrawer
{
    private readonly Warehouse warehouse;
    private readonly SpriteBatch spriteBatch;
    private readonly int tileWidth;
    private readonly int tileHeight;
    private readonly Dictionary<Type, TextureRegion> dictTypeToRegion = new();
    private readonly TextureRegion blank;

    public WarehouseDrawer(Warehouse warehouse, SpriteBatch spriteBatch, TileSet tileSet)
    {
        this.warehouse = warehouse;
        this.spriteBatch = spriteBatch;
        this.tileWidth = tileSet.TileWidth;
        this.tileHeight = tileSet.TileHeight;

        blank = tileSet.GetTile(column: 0, row: 0);
        dictTypeToRegion[typeof(Wall)] = tileSet.GetTile(column: 1, row: 0);
        dictTypeToRegion[typeof(EmptyCell)] = tileSet.GetTile(column: 2, row: 0);
        dictTypeToRegion[typeof(Box)] = tileSet.GetTile(column: 3, row: 0);
        dictTypeToRegion[typeof(BoxArea)] = tileSet.GetTile(column: 4, row: 0);
        dictTypeToRegion[typeof(Storekeeper)] = tileSet.GetTile(column: 5, row: 0);
    }

    public void Draw()
    {
        foreach (var pair in warehouse.AreaMaster.AllAreasWithCoords())
        {
            var coords = pair.Key;
            var area = pair.Value;
            var textureRegion = dictTypeToRegion.GetValueOrDefault(area.GetType()) ?? blank;
            DrawRegion(textureRegion, tileWidth * coords.x,  tileHeight * coords.y);
        }

        for (var x = 0; x < warehouse.MainLayerField.Width; x++)
        for (var y = 0; y < warehouse.MainLayerField.Height; y++)
        {
            var textureRegion = dictTypeToRegion.GetValueOrDefault(warehouse.MainLayerField[x, y].GetType()) ?? blank;
            DrawRegion(textureRegion, tileWidth * x, tileHeight * y);
        }

        foreach (var player in warehouse.DictIdToPlayer.Values)
        {
            var textureRegion = dictTypeToRegion.GetValueOrDefault(player.GetType()) ?? blank;
            DrawRegion(textureRegion, tileWidth * player.X, tileHeight * player.Y);
        }
    }

    private void DrawRegion(TextureRegion textureRegion, int positionX, int positionY)
    {
        textureRegion.Draw(
            spriteBatch,
            new Vector2(positionX, positionY),
            Color.White);
    }
}