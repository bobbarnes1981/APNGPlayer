using System.Xml.Serialization;

namespace RIATestLibrary
{
    public class Startup
    {
        [XmlElement(ElementName = "Message")]
        public Message[] Messages;
    }
}
