namespace Sokoban.Serialization.XmlObjects;

using System.Xml.Serialization;

public class Area
{
    [XmlAttribute("class")]
    public string Class { get; set; }
}