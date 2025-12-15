using Microsoft.Xna.Framework.Graphics;
using Sokoban.Logic;

namespace Sokoban.UI;

public class LevelDrawer
{
    private readonly Level level;
    private readonly SpriteBatch spriteBatch;
    private readonly WarehouseDrawer warehouseDrawer;
    // TODO: добавить отрисовку названия уровня (а потом и других полей)

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