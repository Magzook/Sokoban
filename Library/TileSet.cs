using System.IO;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Sokoban.Library;

public class TileSet
{
    public readonly int TileWidth;
    public readonly int TileHeight;
    public readonly int Columns;
    public readonly int Rows;
    public readonly int Count;
    
    private readonly TextureRegion[] tiles;
    
    private TileSet(TextureRegion tilesAsSingleTextureRegion, int tileWidth, int tileHeight)
    {
        TileWidth = tileWidth;
        TileHeight = tileHeight;
        Columns = tilesAsSingleTextureRegion.Width / tileWidth;
        Rows = tilesAsSingleTextureRegion.Height / tileHeight;
        Count = Columns * Rows;
        tiles = new TextureRegion[Count];
        for (var i = 0; i < Count; i++)
        {
            var x = i % Columns * tileWidth;
            var y = i / Columns * tileHeight;
            tiles[i] = new TextureRegion(
                tilesAsSingleTextureRegion.Texture, 
                tilesAsSingleTextureRegion.SourceRectangle.X + x, 
                tilesAsSingleTextureRegion.SourceRectangle.Y + y, 
                tileWidth, 
                tileHeight);
        }
    }

    public static TileSet FromFile(string fileName, ContentManager content, TextureRegion tilesAsSingleTextureRegion)
    {
        var filePath = Path.Combine(content.RootDirectory, fileName);
        using var stream = TitleContainer.OpenStream(filePath);
        using var reader = XmlReader.Create(stream);
        var doc = XDocument.Load(reader);
        var root = doc.Root;
        
        var tileWidth = int.Parse(root.Attribute("tileWidth").Value);
        var tileHeight = int.Parse(root.Attribute("tileHeight").Value);

        return new (tilesAsSingleTextureRegion, tileWidth, tileHeight);
    }

    public TextureRegion GetTile(int column, int row) 
        => tiles[row * Columns + column];
}