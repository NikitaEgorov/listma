using System;


namespace Listma
{
    /// <summary>
    /// Provides adapter interface for interaction entity with Listma framework
    /// </summary>
    /// <typeparam name="T">entity type</typeparam>
    public interface IWorkflowAdapter<T> 
    {
        /// <summary>
        /// Gets entity type string
        /// </summary>
        string EntityType { get; }

        /// <summary>
        /// Gets current entity state
        /// </summary>
        string CurrentState { get; }

        /// <summary>
        /// Gets statechart Id
        /// </summary>
        string StatechartId { get; }

        /// <summary>
        /// Sets entity type
        /// </summary>
        /// <param name="entityType"></param>
        void SetEntityType(string entityType);

        /// <summary>
        /// Sets current entity state
        /// </summary>
        /// <param name="newState"></param>
        void SetCurrentState(string newState);

        /// <summary>
        /// Sets statechart Id
        /// </summary>
        /// <param name="statechartId"></param>
        void SetStatechartId(string statechartId);

        /// <summary>
        /// Gets wrapped entity
        /// </summary>
        T Entity { get; }
    }
}
