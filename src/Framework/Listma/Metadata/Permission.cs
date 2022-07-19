using System;
using System.Xml.Serialization;

namespace Listma
{
    /// <summary>
    /// UI element permission
    /// </summary>
    [XmlType(Namespace = Statechart.XmlNamespace)]
    public class Permission
    {
        /// <summary>
        /// Role name
        /// </summary>
        [XmlAttribute]
        public string Role = string.Empty;
        /// <summary>
        /// Permission level
        /// </summary>
        [XmlAttribute]
        public UIPermissionLevel Level = UIPermissionLevel.Hidden;
    }
}
