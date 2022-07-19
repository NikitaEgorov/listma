using System;
using System.Xml.Serialization;

namespace Listma
{
    /// <summary>
    /// Descriprion class
    /// </summary>
    [XmlType(Namespace = Statechart.XmlNamespace)]
    public class Description
    {
        /// <summary>
        /// Path to the source file
        /// </summary>
        [XmlAttribute("Src")]
        public string Source = string.Empty;
        /// <summary>
        /// Description text
        /// </summary>
        [XmlText]
        public string Text = string.Empty;
    }
}
