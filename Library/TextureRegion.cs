using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sokoban.Library;

public class TextureRegion
{
    public Texture2D Texture { get; }
    public Rectangle SourceRectangle { get; }

    public int Width => SourceRectangle.Width;

    public int Height => SourceRectangle.Height;
    
    public TextureRegion(Texture2D texture, int x, int y, int width, int height)
    {
        Texture = texture;
        SourceRectangle = new Rectangle(x, y, width, height);
    }
    
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
    {
        Draw(spriteBatch,
            position,
            color, 
            rotation: 0.0f,
            origin: Vector2.Zero,
            scale: Vector2.One,
            SpriteEffects.None,
            layerDepth: 0.0f);
    }
    
    public void Draw(
        SpriteBatch spriteBatch,
        Vector2 position,
        Color color,
        float rotation,
        Vector2 origin,
        float scale,
        SpriteEffects effects,
        float layerDepth)
    {
        Draw(spriteBatch,
            position,
            color,
            rotation,
            origin,
            scale: new Vector2(scale, scale),
            effects,
            layerDepth
        );
    }
    
    public void Draw(
        SpriteBatch spriteBatch,
        Vector2 position,
        Color color,
        float rotation,
        Vector2 origin,
        Vector2 scale,
        SpriteEffects effects,
        float layerDepth)
    {
        spriteBatch.Draw(
            Texture,
            position,
            SourceRectangle,
            color,
            rotation,
            origin,
            scale,
            effects,
            layerDepth
        );
    }
}