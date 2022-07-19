using System;
using System.Xml.Serialization;

namespace Listma.Configuration
{
    /// <summary>
    /// Entity workflow configuration class
    /// </summary>
    [XmlType(Namespace = ListmaConfigurationData.XmlNamespace)]
    public class EntityWorkflow
    {
        /// <summary>
        /// Config data
        /// </summary>
        internal protected EntityWorkflowData _data; 
        internal EntityWorkflow(EntityWorkflowData data)
        {
            _data = data;
        }

        /// <summary>
        /// Initializes a new instance with entity type string, statechart Id and state attribute mapping
        /// </summary>
        /// <param name="entityType">The entity type string that associated entity with given workflow</param>
        /// <param name="statechartId">statechart Id</param>
        /// <param name="stateMap">entity state attribute mapping</param>
        public EntityWorkflow(string entityType, string statechartId, string stateMap)
        {
            _data = new EntityWorkflowData();
            _data.EntityType = entityType;
            _data.StatechartId = statechartId;
            _data.StateMap = stateMap;
        }

        /// <summary>
        /// Sets stateshart Id mapping
        /// </summary>
        /// <param name="statechartMap">The name of entity attribute (field or property) that stores statechart Id</param>
        public void SetStatechartMap(string statechartMap)
        {
            if (statechartMap.IsNullOrEmpty())
                throw new ArgumentNullException("statechartMap");
            _data.StatechartMap = statechartMap;
        }

        /// <summary>
        /// Registers type that implements IWorkflowFactory interface for this workflow at runtime
        /// </summary>
        /// <param name="wfType">The type that implements IWorkflowFactory</param>
        public void RegisterWorkflowFactoryType(Type wfType)
        {
            if (wfType == null)
                throw new ArgumentNullException("WorkflowFactoryType");
            _data.WorkflowFactoryClass = wfType.AssemblyQualifiedName;
        }
        /// <summary>
        /// Registers type that implements IRoleProvider interface for this workflow at runtime
        /// </summary>
        /// <param name="rpType">The type that implements IRoleProvider</param>
        public void RegisterRoleProviderType(Type rpType)
        {
            if (rpType == null)
                throw new ArgumentNullException("RoleProviderType");
            _data.RoleProviderClass = rpType.AssemblyQualifiedName;
        }
        /// <summary>
        /// Sets initial state
        /// </summary>
        /// <param name="stateId">state Id</param>
        public void SetInitialState(string stateId)
        {
            if (stateId.IsNullOrEmpty())
                throw new ArgumentNullException("InitialState");
            _data.InitialState = InitialState;
        }
        /// <summary>
        /// Gets entity type string
        /// </summary>
        public string EntityType { get { return _data.EntityType; } }
        /// <summary>
        /// Gets workflow factory class name
        /// </summary>
        public string WorkflowFactoryClass { get { return _data.WorkflowFactoryClass; } }
        /// <summary>
        /// Gets role provider class name
        /// </summary>
        public string RoleProviderClass { get { return _data.RoleProviderClass; } }
        /// <summary>
        /// Gets workflow statechart Id
        /// </summary>
        public string StatechartId { get { return _data.StatechartId; } }
        /// <summary>
        /// Gets initial state Id
        /// </summary>
        public string InitialState { get { return _data.InitialState; } }
        /// <summary>
        /// Gets entity state attribute name
        /// </summary>
        public string StateMap { get { return _data.StateMap; } }
        /// <summary>
        /// Gets entity attribute name that stores statechart Id 
        /// </summary>
        public string StatechartMap { get { return _data.StatechartMap; } } 
        
    }
}
