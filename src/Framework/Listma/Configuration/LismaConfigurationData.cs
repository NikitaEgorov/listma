using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Listma.Utils;
using System.IO;

namespace Listma.Configuration
{
    /// <summary>
    /// Configuration data class
    /// </summary>
    [XmlRoot("ListmaConfiguration", Namespace = ListmaConfigurationData.XmlNamespace)]
    public class ListmaConfigurationData
    {
        /// <summary>
        /// Xml namespace for serialization
        /// </summary>
        public const string XmlNamespace = "urn:Listma:configuration";

        /// <summary>
        /// Initialized instance
        /// </summary>
        public ListmaConfigurationData() 
        {
            StatechartDir = AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// Initialized instance with location of statecharts directory
        /// </summary>
        /// <param name="statechartDir"></param>
        public ListmaConfigurationData(string statechartDir)
        {
            StatechartDir = statechartDir;
        }

        /// <summary>
        /// Statecharts directory
        /// </summary>
        [XmlAttribute]
        public string StatechartDir = string.Empty;

        /// <summary>
        /// Default permissions level
        /// </summary>
        [XmlAttribute]
        public UIPermissionLevel DefaultPermissionLevel = UIPermissionLevel.Hidden;

        List<EntityWorkflowData> _EntitiesData = new List<EntityWorkflowData>();
        /// <summary>
        /// Entities workflow configuration data
        /// </summary>
        [XmlElement("EntityWorkflow")]
        public EntityWorkflowData[] EntitiesData
        {
            get
            {
                return _EntitiesData.ToArray();
            }
            set
            {
                _EntitiesData = new List<EntityWorkflowData>();
                if (value != null)
                {
                    foreach (EntityWorkflowData e in value) _EntitiesData.Add(e);
                }
                _Entities = null;
            }
        }
        
        List<EntityWorkflow> _Entities = null;
        /// <summary>
        /// Entities workflow
        /// </summary>
        [XmlIgnore]
        public EntityWorkflow[] Entities
        {
            get 
            {
                if (_Entities == null)
                {
                    _Entities = new List<EntityWorkflow>();
                    foreach (EntityWorkflowData data in EntitiesData)
                        _Entities.Add(new EntityWorkflow(data));
                }
                return _Entities.ToArray();
            }
        }

        /// <summary>
        /// Gets default configuration file location
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultConfigLocation()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Listma.config");
        }
        /// <summary>
        /// Loads configuration data from given file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static ListmaConfigurationData Load(string fileName)
        {
            ListmaConfigurationData result = null;
            if (File.Exists(fileName))
            {
                using (TextReader reader = new StreamReader(fileName))
                {
                    result = ListmaConfigurationData.ConstructFromXml(reader.ReadToEnd());

                    reader.Close();
                }
            }
            else
                throw new WorkflowException("Listma configuration failed. Config file '{0}' does not exist.", fileName);
            return result;
        }
        
        static ListmaConfigurationData ConstructFromXml(string xml)
        {
            return XmlUtility.XmlStr2Obj<ListmaConfigurationData>(xml);
        }

        internal EntityWorkflow GetEntityWorkflow(string entityType)
        {
            foreach (EntityWorkflow e in Entities)
                if (e.EntityType == entityType) return e;
            throw new WorkflowException("Entity type '{0}' is not registered.", entityType);
        }

        internal string GetEntityWorkflowFactoryName(string entityType)
        {
            foreach (EntityWorkflow e in Entities)
                if (e.EntityType == entityType) return e.WorkflowFactoryClass;
            throw new WorkflowException("Entity type '{0}' is not registered.", entityType);
        }

        internal void AddEntityWorkflow(EntityWorkflow entityWorkflow)
        {
            if (entityWorkflow == null)
                throw new ArgumentNullException("orderWorkflow");
            if (IsEntityTypeRegistered(entityWorkflow.EntityType))
                throw new WorkflowException("Workflow for entity type '{0}' already registered.", entityWorkflow.EntityType);
            _Entities.Add(entityWorkflow);
            _EntitiesData.Add(entityWorkflow._data);
        }

        internal string GetRolePrviderName(string entityType)
        {
            foreach (EntityWorkflow e in Entities)
                if (e.EntityType == entityType) return e.RoleProviderClass;
            throw new WorkflowException("Entity type '{0}' is not registered.", entityType);
        }

        internal bool IsEntityTypeRegistered(string entityType)
        {
            foreach (EntityWorkflow e in Entities)
                if (e.EntityType == entityType) return true;
            return false;
        }
    }
}
