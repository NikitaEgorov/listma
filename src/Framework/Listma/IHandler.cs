using System;

namespace Listma
{
    /// <summary>
    /// Provides generic interface for statecharts events handlers  
    /// </summary>
    /// <typeparam name="EntityType">entity type</typeparam>
    /// <typeparam name="ContextType">call context type</typeparam>
    public interface IHandler<EntityType, ContextType>
    {
        /// <summary>
        /// This method is called when statechart event raises 
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="context">call context</param>
        void Execute(EntityType entity, ContextType context);
    }
}
