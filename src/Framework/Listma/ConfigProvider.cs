using System;

using Listma.Configuration;
using System.IO;
using Listma;

namespace Listma
{
    /// <summary>
    /// Default implementation of IConfigProvider interface
    /// </summary>
    /// <remarks>
    /// ConfigProvider caches Statechart objects internall.  
    /// </remarks>
    public class ConfigProvider : IConfigProvider
    {
        #region Default singleton
        static IConfigProvider _default;
        static object syncRoot = new object(); 
        /// <summary>
        /// Returns singleton instance which loads configuration data from "listma.config" file in the AppDomain.BaseDirectory
        /// </summary>
        /// <returns>ConfigProvider singletone instance</returns>
        public static IConfigProvider GetDefault()
        {
            if (_default == null)
            {
                lock (syncRoot)
                {

                    if (_default == null) _default = new ConfigProvider(
                        ListmaConfigurationData.Load(
                        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "listma.config")));
                }
            }
            return _default;
        }
        #endregion

        private ListmaConfigurationData _config;
        private StatechartCache _statechartCache;
        private StatechartCache StatechartCache
        {
            get 
            {
                if (_statechartCache == null)
                {
                    _statechartCache = new StatechartCache(_config.StatechartDir);
                }
                return _statechartCache;
            }
        }

        /// <summary>
        /// Default parameterless constructor. Creates empty configuration 
        /// </summary>
        public ConfigProvider()
        {
            _config = new ListmaConfigurationData();
        }

        /// <summary>
        /// Creates instance and loads configuration data from given file
        /// </summary>
        /// <param name="configFileName">configuration file name</param>
        public ConfigProvider(string configFileName)
        {
            _config = ListmaConfigurationData.Load(configFileName);
        }

        internal ConfigProvider(ListmaConfigurationData config) 
        {
            _config = config;
        }

       
        #region IConfigProvider Members

        /// <summary>
        /// Sets directory location for loading statechart xml files 
        /// </summary>
        /// <param name="statechartDir">directory location (path)</param>
        public void SetStatechartDir(string statechartDir)
        {
            if (_statechartCache != null)
                throw new WorkflowException("Can not set StatechartDir because statechart cashe has already been initialized.");
            _statechartCache = new StatechartCache(statechartDir);
        }

        /// <summary>
        /// Returns entity workflow configuration for given entity type
        /// </summary>
        /// <param name="entityType">entity type string</param>
        /// <returns>entity workflow</returns>
        public EntityWorkflow GetEntityWorkflow(string entityType)
        {
            return _config.GetEntityWorkflow(entityType);
        }

        /// <summary>
        /// Returns IWorkflowFactory for given entity type
        /// </summary>
        /// <typeparam name="EntityType">entity type</typeparam>
        /// <param name="entityType">entity type string</param>
        /// <returns>IWorkflowFactory</returns>
        public IWorkflowFactory<EntityType> GetWorkflowFactory<EntityType>(string entityType)
        {
            return ReflectedTypeCache.GetInstance<IWorkflowFactory<EntityType>>(
                _config.GetEntityWorkflowFactoryName(entityType), new[]{typeof(EntityType)});
        }

        /// <summary>
        /// Returns IRoleProvider for given entity type
        /// </summary>
        /// <typeparam name="EntityType">entity type</typeparam>
        /// <typeparam name="ContextType">call context type</typeparam>
        /// <param name="entityType">entity type string</param>
        /// <returns>IRoleProvider interface</returns>
        public IRoleProvider<EntityType,ContextType> GetRoleProvider<EntityType, ContextType>(string entityType)
        {
            return ReflectedTypeCache.GetInstance<IRoleProvider<EntityType, ContextType>>(
                _config.GetRolePrviderName(entityType), new Type[] { typeof(EntityType), typeof(ContextType) });
        }

        /// <summary>
        /// Returns statechart by its Id
        /// </summary>
        /// <param name="statechartId">statechart Id</param>
        /// <returns>statechart</returns>
        public Statechart GetStatechart(string statechartId)
        {
            return StatechartCache.GetStatechart(statechartId);
        }

     
        /// <summary>
        /// Registers entity workflow in the configuration at runtime
        /// </summary>
        /// <param name="entityWorkflow">entity workflow</param>
        public void RegisterEntityWorkflow(EntityWorkflow entityWorkflow)
        {
            _config.AddEntityWorkflow(entityWorkflow);
        }

        /// <summary>
        /// Gets or sets default UIPermissionLevel for UI elements that are not configured in statechart
        /// </summary>
        public UIPermissionLevel DefaultPermissionLevel 
        {
            get { return _config.DefaultPermissionLevel; }
            set { _config.DefaultPermissionLevel = value; }
        }

        /// <summary>
        /// Registers statechart in configuration at Runtime
        /// </summary>
        /// <param name="statechart">statechart</param>
        public void RegisterStatechart(Statechart statechart)
        {
            StatechartCache.AddStatechart(statechart);
        }

        #endregion

    }
}
