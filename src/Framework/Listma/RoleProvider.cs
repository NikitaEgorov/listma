using System;


namespace Listma
{
    /// <summary>
    /// Default implementation of IRoleProvider interface
    /// </summary>
    /// <typeparam name="EntityType">entity type</typeparam>
    /// <typeparam name="ContextType">call context type</typeparam>
    public class RoleProvider<EntityType, ContextType> : IRoleProvider<EntityType,ContextType>
    {
        #region IRoleProvider Members
        /// <summary>
        /// Gets a value indicating whether the current user is in the specified role in context of entity instance.
        /// </summary>
        /// <param name="role">role name</param>
        /// <param name="entity">entity</param>
        /// <param name="context">call context object</param>
        /// <returns>true if the user is in the specified role; false if the user is not in the specified role or is not authenticated.</returns>
        public bool IsInRole(string role, EntityType entity, ContextType context)
        {
            return System.Threading.Thread.CurrentPrincipal.IsInRole(role);
        }

        #endregion
    }
}
