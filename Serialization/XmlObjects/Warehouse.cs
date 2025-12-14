using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sokoban.Serialization.XmlObjects;

public class Warehouse
{
    [XmlElement("MainLayerField")]
    public MainLayerField MainLayerField { get; set; }
    
    [XmlArray("AreaEntries")]
    [XmlArrayItem("AreaEntry")]
    public List<AreaEntry> AreaEntries { get; set; }
    
    [XmlArray("PlayableCharacters")]
    [XmlArrayItem("PlayableCharacter")]
    public List<PlayableCharacter> PlayableCharacters { get; set; }
}