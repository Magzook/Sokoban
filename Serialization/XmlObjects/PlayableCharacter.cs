using System.Xml.Serialization;

namespace Sokoban.Serialization.XmlObjects;

public class PlayableCharacter
{
    [XmlAttribute("id")]
    public int Id { get; set; }
    
    [XmlAttribute("x")]
    public int X { get; set; }
    
    [XmlAttribute("y")]
    public int Y { get; set; }
    
    [XmlAttribute("class")]
    public string Class { get; set; }
}