using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using Gum.Forms.Controls;
using Sokoban.Library;

namespace Sokoban.Scenes;

public class TitleScene : Scene
{
    private const string SOKOBAN_TEXT = "Sokoban";

    // The font to use to render normal text.
    private SpriteFont _font;

    // The position to draw the dungeon text at.
    private Vector2 sokobanTextPos;

    // The origin to set for the dungeon text.
    private Vector2 sokobanTextOrigin;
    
    public override void Initialize()
    {
        // LoadContent is called during base.Initialize().
        base.Initialize();

        // Set the position and origin for the Dungeon text.
        Vector2 size = _font.MeasureString(SOKOBAN_TEXT);
        sokobanTextPos = new Vector2(640, 100);
        sokobanTextOrigin = size * 0.5f;
        
        InitializeUi();
    }

    public override void LoadContent()
    {
        // Load the font for the standard text.
        _font = Core.Content.Load<SpriteFont>("fonts/font");
    }
    
    public override void Update(GameTime gameTime)
    {
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Escape))
            Core.Instance.Exit();
    }

    public override void Draw(GameTime gameTime)
    {
        // Draw the Dungeon text on top of that at its original position.
        Core.SpriteBatch.DrawString(
            _font, 
            SOKOBAN_TEXT,
            sokobanTextPos,
            Color.White,
            0.0f,
            sokobanTextOrigin,
            1.0f,
            SpriteEffects.None,
            1.0f);
    }

    private void CreateTitlePanel()
    {
        // Create a container to hold all of our buttons
        var buttonsPanel = new Panel();
        buttonsPanel.Dock(Gum.Wireframe.Dock.Fill);
        buttonsPanel.AddToRoot();

        var btnPlay = new Button();
        btnPlay.Anchor(Gum.Wireframe.Anchor.BottomLeft);
        btnPlay.Visual.X = 50;
        btnPlay.Visual.Y = -12;
        btnPlay.Visual.Width = 70;
        btnPlay.Text = "Play";
        btnPlay.Click += OnClickPlay;
        buttonsPanel.AddChild(btnPlay);
        
        var btnEditor = new Button();
        btnEditor.Anchor(Gum.Wireframe.Anchor.BottomRight);
        btnEditor.Visual.X = -50;
        btnEditor.Visual.Y = -12;
        btnEditor.Visual.Width = 70;
        btnEditor.Text = "Editor";
        btnEditor.Click += OnClickEditor;
        buttonsPanel.AddChild(btnEditor);
    }

    private void OnClickPlay(object sender, EventArgs e)
    {
        Core.ChangeScene(new PlayScene());
    }
    
    private void OnClickEditor(object sender, EventArgs e)
    {
        Core.ChangeScene(new EditorScene());
    }
    
    private void InitializeUi()
    {
        // Clear out any previous UI in case we came here from
        // a different screen:
        GumService.Default.Root.Children.Clear();

        CreateTitlePanel();
    }
}