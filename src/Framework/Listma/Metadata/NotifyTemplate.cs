using System;
using System.Xml.Serialization;

namespace Listma
{
    /// <summary>
    /// Notification message template
    /// </summary>
    [XmlType(Namespace = Statechart.XmlNamespace)]
    public class NotifyTemplate
    {
        /// <summary>
        /// Template Id
        /// </summary>
        [XmlAttribute]
        public string Id = string.Empty;
        /// <summary>
        /// Subject template
        /// </summary>
        [XmlElement]
        public string Subject = string.Empty;
        /// <summary>
        /// Body template
        /// </summary>
        [XmlElement]
        public string Body = string.Empty;
    }
}
