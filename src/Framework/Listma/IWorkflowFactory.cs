using System;

using Listma.Configuration;

namespace Listma
{
    /// <summary>
    /// Provides generic factory interface for IWorkflowAdapter 
    /// </summary>
    /// <typeparam name="EntityType">entity type</typeparam>
    public interface IWorkflowFactory<EntityType>
    {
        /// <summary>
        /// Creates IWorkflowAdapter for given entity
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="stateMap">entity attribute name (field or property) which stores entities state</param>
        /// <returns>IWorkflowAdapter foe given entity</returns>
        IWorkflowAdapter<EntityType> GetWorkflow(EntityType entity, string stateMap);

        /// <summary>
        /// Provides generic factory interface for IWorkflowAdapter 
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="stateMap">entity attribute name (field or property) which stores entities state</param>
        /// <param name="statechartMap">entity attribute name (field or property) which stores entities stateshart Id</param>
        /// <returns>IWorkflowAdapter foe given entity</returns>
        /// <remarks>
        /// This method is invoked by framework when StatechartMap attribute is defined
        /// in the entity workflow configuration. StatechartMap provides statechart versioning support for 
        /// different instances of the entity.
        /// </remarks>
        IWorkflowAdapter<EntityType> GetWorkflow(EntityType entity, string stateMap, string statechartMap);
    }
}
