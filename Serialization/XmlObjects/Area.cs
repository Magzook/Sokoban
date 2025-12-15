using System.Xml.Serialization;

namespace Sokoban.Serialization.XmlObjects;

public class Area
{
    [XmlAttribute("class")]
    public string Class { get; set; }
}