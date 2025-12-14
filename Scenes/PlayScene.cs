using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using Sokoban.Library;
using Sokoban.Logic;
using Sokoban.Serialization;
using Sokoban.UI;

namespace Sokoban.Scenes;

public class PlayScene : Scene
{
    private Level level;
    private LevelDrawer levelDrawer;
    
    private int previousKeysPressedCount = 0;
    
    public override void Initialize()
    {
        base.Initialize();
        InitializeUi();
    }

    public override void LoadContent()
    {
        base.LoadContent();

        var textureAtlas = TextureAtlas.FromFile("images/atlas-definition.xml", Content);
        var tilesAsSingleTextureRegion = textureAtlas.GetRegion("fieldTiles");
        var tileSet = TileSet.FromFile("images/fieldTiles-definition.xml", Content, tilesAsSingleTextureRegion);
        
        var serializer = new XmlSerializer(typeof(Serialization.XmlObjects.Level));
        using (var fs = new FileStream(Path.Combine(Content.RootDirectory, "levels/levelFromEditor.xml"), FileMode.Open))
        {
            var dataLevel = (Serialization.XmlObjects.Level)serializer.Deserialize(fs);
            level = Mapper.LevelFromDataObject(dataLevel);
        }
        
        levelDrawer = new LevelDrawer(
            level, 
            Core.SpriteBatch, 
            new WarehouseDrawer(level.Warehouse, Core.SpriteBatch, tileSet)
        );
    }

    public override void Update(GameTime gameTime)
    {
        var keyboard = Core.Input.Keyboard;

        if (keyboard.WasKeyJustPressed(Keys.Escape))
            Core.ChangeScene(new TitleScene());

        if (keyboard.WasKeyJustPressed(Keys.W))
            level.OnPressDirection(1, Direction.Up);
        else if (keyboard.WasKeyJustPressed(Keys.S))
            level.OnPressDirection(1, Direction.Down);
        else if (keyboard.WasKeyJustPressed(Keys.A))
            level.OnPressDirection(1, Direction.Left);
        else if (keyboard.WasKeyJustPressed(Keys.D))
            level.OnPressDirection(1, Direction.Right);
    }

    public override void Draw(GameTime gameTime)
    {
        levelDrawer.Draw();
        
        base.Draw(gameTime);
    }
    
    private void InitializeUi()
    {
        // Clear out any previous UI in case we came here from
        // a different screen:
        GumService.Default.Root.Children.Clear();
    }
}