using System;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using Gum.Forms.Controls;
using Sokoban.Library;
using Sokoban.Logic;
using Sokoban.Logic.WarehouseObjects.AreaLayer;
using Sokoban.Logic.WarehouseObjects.AreaLayer.Areas;
using Sokoban.Logic.WarehouseObjects.MainLayer;
using Sokoban.Logic.WarehouseObjects.MainLayer.Cells;
using Sokoban.Logic.WarehouseObjects.PlayerLayer;
using Sokoban.Logic.WarehouseObjects.PlayerLayer.PlayableCharacters;
using Sokoban.Serialization;
using Sokoban.UI;

namespace Sokoban.Scenes;

public class EditorScene : Scene
{
    private Warehouse warehouse;
    private WarehouseDrawer warehouseDrawer;
    private TileSet tileSet;
    
    private ICell selectedCell = EmptyCell.Get();
    private AddingMode addingMode = AddingMode.AddCell;
    private IArea selectedArea;
    private PlayableCharacter selectedCharacter;
    
    private const int WAREHOUSE_WIDTH = 10; // размеры слада, в теории можно сделать намного больше
    private const int WAREHOUSE_HEIGHT = 7;
    
    public override void Initialize()
    {
        // LoadContent is called during base.Initialize().
        base.Initialize();
        
        InitializeUi();
    }
    
    public override void LoadContent()
    {
        base.LoadContent();
        
        var textureAtlas = TextureAtlas.FromFile("images/atlas-definition.xml", Content);
        var tilesAsSingleTextureRegion = textureAtlas.GetRegion("fieldTiles");
        tileSet = TileSet.FromFile("images/fieldTiles-definition.xml", Content, tilesAsSingleTextureRegion);

        warehouse = Warehouse.MakeFilledWithWalls(WAREHOUSE_WIDTH, WAREHOUSE_HEIGHT);
        warehouseDrawer = new WarehouseDrawer(warehouse, Core.SpriteBatch, tileSet);
    }
    
    public override void Update(GameTime gameTime)
    {
        var mouse = Core.Input.Mouse;
        if (mouse.IsButtonDown(MouseButton.Left))
        {
            var mousePos = mouse.CurrentState.Position;
            var mouseX = mousePos.X;
            var mouseY = mousePos.Y;
            var targetCellX = mouseX / tileSet.TileWidth;
            var targetCellY = mouseY / tileSet.TileHeight;
            if (targetCellX < WAREHOUSE_WIDTH && targetCellY < WAREHOUSE_HEIGHT && targetCellX >= 0 && targetCellY >= 0)
            {
                switch (addingMode)
                {
                    case AddingMode.AddCell:
                        warehouse.MainLayerField[targetCellX, targetCellY] = selectedCell;
                        break;
                    case AddingMode.AddArea:
                        warehouse.AreaMaster.SetAreaAt(targetCellX, targetCellY, selectedArea);
                        break;
                    case AddingMode.AddCharacter:
                        var player = new Storekeeper(1, targetCellX,  targetCellY);
                        warehouse.DictIdToPlayer[player.Id] = player;
                        break;
                }
            }
        }
        
        var keyboard = Core.Input.Keyboard;
        if (keyboard.WasKeyJustPressed(Keys.Escape))
            Core.ChangeScene(new TitleScene());

        if (keyboard.WasKeyJustPressed(Keys.D1))
        {
            selectedCell = EmptyCell.Get();
            addingMode = AddingMode.AddCell;
        }
        else if (keyboard.WasKeyJustPressed(Keys.D2))
        {
            selectedCell = Wall.Get();
            addingMode = AddingMode.AddCell;
        }
        else if (keyboard.WasKeyJustPressed(Keys.D3))
        {
            selectedCell = Box.Get();
            addingMode = AddingMode.AddCell;
        }
        else if (keyboard.WasKeyJustPressed(Keys.D4))
        {
            selectedArea = BoxArea.Get();
            addingMode = AddingMode.AddArea;
        }
        else if (keyboard.WasKeyJustPressed(Keys.D5))
        {
            addingMode = AddingMode.AddCharacter;
        }
    }
    
    public override void Draw(GameTime gameTime)
    {
        // TODO: нарисовать сетку под склад
        // TODO: нарисовать инструкцию по использованию, какие кнопки на клавиатуре нажимать и пр.
        
        warehouseDrawer.Draw();
    }
    
    private void CreateEditorPanel()
    {
        var buttonsPanel = new Panel();
        buttonsPanel.Anchor(Gum.Wireframe.Anchor.TopRight);
        buttonsPanel.AddToRoot();
        
        var btnSave = new Button
        {
            Text = "Save",
        };
        btnSave.Click += OnClickSave;
        buttonsPanel.AddChild(btnSave);
    }
    
    private void OnClickSave(object sender, EventArgs e)
    {
        var dataLevel = Mapper.LevelToDataObject(new Level("", warehouse));
        var serializer = new XmlSerializer(typeof(Serialization.XmlObjects.Level));
        // TODO: заменить на папку проекта, добавить хотя бы на файловый диалог, чтоб можно было сохранять много уровней
        using (var fs = new FileStream(Path.Combine(Content.RootDirectory, "levels/levelFromEditor.xml"), FileMode.Create))
        {
            serializer.Serialize(fs, dataLevel);
        }
    }
    
    private void InitializeUi()
    {
        // Clear out any previous UI in case we came here from
        // a different screen:
        GumService.Default.Root.Children.Clear();

        CreateEditorPanel();
    }

    private enum AddingMode
    {
        AddCell,
        AddArea,
        AddCharacter
    }
}