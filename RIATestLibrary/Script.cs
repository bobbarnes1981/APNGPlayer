using System;
using System.Xml.Serialization;

namespace RIATestLibrary
{
    public class Script
    {
        [XmlAttribute(AttributeName = "File")]
        public string File;
        [XmlAttribute(AttributeName = "Time")]
        public string DateTimeString
        {
            get { return DateTime.ToString(); }
            set { DateTime = DateTime.Parse(value); }
        }
        public DateTime DateTime;
        [XmlElement(ElementName = "Message")]
        public Message[] Messages;
    }
}
