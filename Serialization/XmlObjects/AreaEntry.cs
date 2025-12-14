using System.Xml.Serialization;

namespace Sokoban.Serialization.XmlObjects;

public class AreaEntry
{
    [XmlAttribute("x")]
    public int X { get; set; }
    
    [XmlAttribute("y")]
    public int Y { get; set; }
    
    [XmlElement("Area")]
    public Area Area { get; set; }
}