using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Listma
{
    /// <summary>
    /// Provides interface for statechart transition's notification messages handling
    /// </summary>
    /// <typeparam name="EntityType">entity type</typeparam>
    /// <typeparam name="ContextType">call context type</typeparam>
    public interface INotifyHandler<EntityType, ContextType>
    {
        /// <summary>
        /// Resolves recipient's addresses which are specified for roles
        /// </summary>
        /// <param name="role">role name</param>
        /// <param name="entity">entity </param>
        /// <param name="context">call context</param>
        /// <returns>resulting addresses array</returns>
        string[] ResolveAddress(string role, EntityType entity, ContextType context);

        /// <summary>
        /// Parses notification message subject and body templates
        /// </summary>
        /// <param name="message">resulting message</param>
        /// <param name="template">source message template</param>
        /// <param name="entity">entity</param>
        /// <param name="context">call context</param>
        void ParseMessageTemplate(NotifyMessage message, NotifyTemplate template, EntityType entity, ContextType context); 
    }
}
