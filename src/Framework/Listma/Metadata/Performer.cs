using System;
using System.Xml.Serialization;

namespace Listma
{
    /// <summary>
    /// Performer
    /// </summary>
    [XmlType(Namespace = Statechart.XmlNamespace)]
    public class Performer
    {
        /// <summary>
        /// Performer role
        /// </summary>
        [XmlAttribute]
        public string Role = string.Empty;
    }
}
