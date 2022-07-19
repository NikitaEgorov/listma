using System;
using Listma.Configuration;


namespace Listma
{
    /// <summary>
    /// Default implementation of  IWorkflowFactory interface
    /// </summary>
    /// <typeparam name="EntityType">entity type</typeparam>
    public class ReflectionFactory<EntityType>: IWorkflowFactory<EntityType>
    {


        #region IWorkflowFactory Members

        /// <summary>
        /// Creates IWorkflowAdapter for given entity
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="stateMap">entity attribute name (field or property) which stores entities state</param>
        /// <returns>IWorkflowAdapter foe given entity</returns>
        public IWorkflowAdapter<EntityType> GetWorkflow(EntityType entity, string stateMap)
        {
            return new ReflectionEntityWorkflow<EntityType>(entity, stateMap);
        }

        /// <summary>
        /// Provides generic factory interface for IWorkflowAdapter 
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="stateMap">entity attribute name (field or property) which stores entities state</param>
        /// <param name="statechartMap">entity attribute name (field or property) which stores entities stateshart Id</param>
        /// <returns>IWorkflowAdapter foe given entity</returns>
        public IWorkflowAdapter<EntityType> GetWorkflow(EntityType entity, string stateMap, string statechartMap)
        {
            return new ReflectionEntityWorkflow<EntityType>(entity, stateMap, statechartMap);
        }

        #endregion
    }
}
