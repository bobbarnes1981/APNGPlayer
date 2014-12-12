using System;
using System.Xml.Serialization;

namespace RIATestLibrary
{
    [XmlRoot(ElementName = "Report")]
    public class Report
    {
        [XmlAttribute(AttributeName = "App")]
        public string Application;
        [XmlAttribute(AttributeName = "Version")]
        public string Version;
        [XmlAttribute(AttributeName = "Project")]
        public string Project;
        [XmlAttribute(AttributeName = "Time")]
        public string DateTimeString
        { 
            get { return DateTime.ToString(); }
            set { DateTime = DateTime.Parse(value); }
        }
        public DateTime DateTime;
        [XmlElement(ElementName = "Startup")]
        public Startup Startup;
        [XmlArray(ElementName = "Scripts")]
        [XmlArrayItem(ElementName = "Group")]
        public Group[] Groups;
    }
}
