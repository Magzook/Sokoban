namespace Sokoban.UI;

using Microsoft.Xna.Framework.Graphics;
using Logic;

public class LevelDrawer
{
    private readonly Level level;
    private readonly SpriteBatch spriteBatch;
    private readonly WarehouseDrawer warehouseDrawer;

    public LevelDrawer(Level level, SpriteBatch spriteBatch, WarehouseDrawer warehouseDrawer)
    {
        this.level = level;
        this.spriteBatch = spriteBatch;
        this.warehouseDrawer = warehouseDrawer;
    }

    public void Draw()
    {
        warehouseDrawer.Draw();
    }
}