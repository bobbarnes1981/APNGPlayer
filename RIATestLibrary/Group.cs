using System.Xml.Serialization;

namespace RIATestLibrary
{
    public class Group
    {
        [XmlAttribute(AttributeName = "Label")]
        public string Label;
        [XmlElement(ElementName = "Script")]
        public Script[] Scripts;
        [XmlElement(ElementName = "Group")]
        public Group[] SubGroups;
    }
}
