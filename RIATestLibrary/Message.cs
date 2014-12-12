using System;
using System.Xml.Serialization;

namespace RIATestLibrary
{
    public class Message
    {
        [XmlAttribute(AttributeName = "Time")]
        public string DateTimeString
        {
            get { return DateTime.ToString(); }
            set { DateTime = DateTime.Parse(value); }
        }
        public DateTime DateTime;
        [XmlAttribute(AttributeName = "Type")]
        public MessageType Type;
        [XmlAttribute(AttributeName = "Message")]
        public string Text;
        [XmlAttribute(AttributeName = "Line")]
        public int Row;
        [XmlAttribute(AttributeName = "Col")]
        public int Col;
    }
}
