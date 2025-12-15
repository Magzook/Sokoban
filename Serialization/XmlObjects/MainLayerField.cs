using System.Xml.Serialization;

namespace Sokoban.Serialization.XmlObjects;

public class MainLayerField
{
    [XmlText]
    public string FieldData { get; set; }
}