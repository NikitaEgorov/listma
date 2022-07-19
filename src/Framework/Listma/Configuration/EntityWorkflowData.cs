using System;
using System.Xml.Serialization;

namespace Listma.Configuration
{
    /// <summary>
    /// Class for store entity workflow configuration data
    /// </summary>
    [XmlType("EntityWorkflow", Namespace = ListmaConfigurationData.XmlNamespace)]
    public class EntityWorkflowData
    {
        /// <summary>
        /// Entity type string
        /// </summary>
        [XmlAttribute]
        public string EntityType = string.Empty;
        /// <summary>
        /// Workflow factory class
        /// </summary>
        [XmlAttribute]
        public string WorkflowFactoryClass = typeof(ReflectionFactory<>).FullName;
        /// <summary>
        /// Role provider class
        /// </summary>
        [XmlAttribute]
        public string RoleProviderClass = typeof(RoleProvider<,>).FullName;
        /// <summary>
        /// Statechart Id
        /// </summary>
        [XmlAttribute]
        public string StatechartId = string.Empty;
        /// <summary>
        /// Initial state Id
        /// </summary>
        [XmlAttribute]
        public string InitialState = "*";
        /// <summary>
        /// State attribute mapping
        /// </summary>
        [XmlAttribute]
        public string StateMap = string.Empty;
        /// <summary>
        /// Statechart attribute mapping
        /// </summary>
        [XmlAttribute]
        public string StatechartMap = string.Empty;
    }
}
