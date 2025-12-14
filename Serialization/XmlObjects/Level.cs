namespace Sokoban.Serialization.XmlObjects;
using System.Xml.Serialization;

[XmlRoot("Level")]
public class Level
{
    [XmlAttribute("Name")]
    public string Name { get; set; }
    
    [XmlElement("Warehouse")]
    public Warehouse Warehouse { get; set; }
}
