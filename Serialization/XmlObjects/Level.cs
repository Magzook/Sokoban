using System.Xml.Serialization;

namespace Sokoban.Serialization.XmlObjects;

[XmlRoot("Level")]
public class Level
{
    [XmlAttribute("Name")]
    public string Name { get; set; }
    
    [XmlElement("Warehouse")]
    public Warehouse Warehouse { get; set; }
}
