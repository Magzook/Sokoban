using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sokoban.Library;

public class TextureAtlas
{
    private readonly Texture2D texture;
    
    private readonly Dictionary<string, TextureRegion> regions;

    private TextureAtlas(Texture2D texture)
    {
        this.texture = texture;
        regions = new Dictionary<string, TextureRegion>();
    }
    
    public static TextureAtlas FromFile(string fileName, ContentManager content)
    {
        var filePath = Path.Combine(content.RootDirectory, fileName);
        using var stream = TitleContainer.OpenStream(filePath);
        using var reader = XmlReader.Create(stream);
        var doc = XDocument.Load(reader);
        var root = doc.Root;
        
        var texturePath = root.Element("Path").Value;
        var texture = content.Load<Texture2D>(texturePath);
        var atlas = new TextureAtlas(texture);
        
        var regions = root.Element("Regions")?.Elements("Region");
        if (regions != null)
        {
            foreach (var region in regions)
            {
                try
                {
                    var name = region.Attribute("name").Value;
                    var x = int.Parse(region.Attribute("x").Value);
                    var y = int.Parse(region.Attribute("y").Value);
                    var width = int.Parse(region.Attribute("width").Value);
                    var height = int.Parse(region.Attribute("height").Value);
                    atlas.AddRegion(name, x, y, width, height);
                }
                catch (NullReferenceException)
                {
                    Console.WriteLine($"{filePath}: a Region is missing an attribute");
                }
            }
        }

        return atlas;
    }
    
    public TextureRegion GetRegion(string name)
        => regions[name];
    
    private void AddRegion(string name, int x, int y, int width, int height)
    {
        var region = new TextureRegion(texture, x, y, width, height);
        regions.Add(name, region);
    }
}