using System;
using Listma.Configuration;
using Listma;

namespace Listma
{
    /// <summary>
    /// Provides base interface for manage Listma framework configuration
    /// </summary>
    public interface IConfigProvider
    {
        /// <summary>
        /// Sets directory for load statechart files
        /// </summary>
        /// <param name="statechartDir"></param>
        void SetStatechartDir(string statechartDir);

        /// <summary>
        /// Registers workflow configuration for entity type
        /// </summary>
        /// <param name="entityWorkflow"></param>
        void RegisterEntityWorkflow(EntityWorkflow entityWorkflow);
        
        /// <summary>
        /// Registers statechart in runtime  
        /// </summary>
        /// <param name="statechart"></param>
        void RegisterStatechart(Statechart statechart);

        /// <summary>
        /// Gets or sets UIPermissionLevel for UI elements that aren't specified in the statechart 
        /// </summary>
        UIPermissionLevel DefaultPermissionLevel { get; set; }

        /// <summary>
        /// Returns workflow configuration for entity type
        /// </summary>
        /// <param name="entityType">entity type string</param>
        /// <returns>entity workflow configuration</returns>
        EntityWorkflow GetEntityWorkflow(string entityType);

        /// <summary>
        /// Returns workflow adapter factory for entity type
        /// </summary>
        /// <typeparam name="EntityType">entity type </typeparam>
        /// <param name="entityType">entity type string</param>
        /// <returns>workflow adapter factory instance</returns>
        IWorkflowFactory<EntityType> GetWorkflowFactory<EntityType>(string entityType);

        /// <summary>
        /// Returns roles provider for entity type
        /// </summary>
        /// <typeparam name="EntityType">entity type</typeparam>
        /// <typeparam name="ContextType">call context type</typeparam>
        /// <param name="entityType">entity type string</param>
        /// <returns>roles provider instance</returns>
        IRoleProvider<EntityType, ContextType> GetRoleProvider<EntityType, ContextType>(string entityType);

        /// <summary>
        /// Returns statechart by Id
        /// </summary>
        /// <param name="statechartId">statechart Id</param>
        /// <returns>statechart</returns>
        Statechart GetStatechart(string statechartId);
    }
}
