namespace Sokoban.Serialization.XmlObjects;
using System.Xml.Serialization;

public class MainLayerField
{
    [XmlText]
    public string FieldData { get; set; }
}