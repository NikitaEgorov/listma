using System;

namespace Listma
{
    /// <summary>
    /// Default implementation of the IPermissionProvider interface
    /// </summary>
    /// <typeparam name="EntityType">entity type</typeparam>
    /// <typeparam name="ContextType">call context</typeparam>
    public class PermissionProvider<EntityType, ContextType> : IPermissionProvider
    {
        IWorkflowAdapter<EntityType> _entity;
        IRoleProvider<EntityType, ContextType> _roleProvider;
        ContextType _context;
        Statechart _statechart;
        UIPermissionLevel _defaultLevel;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="roleProvider">role provider Interface</param>
        /// <param name="statechart">statechart</param>
        /// <param name="defaultLevel">default permission level</param>
        /// <param name="context">call context</param>
        public PermissionProvider(IWorkflowAdapter<EntityType> entity, IRoleProvider<EntityType, ContextType> roleProvider, Statechart statechart, UIPermissionLevel defaultLevel, ContextType context)
        {
            _entity = entity;
            _roleProvider = roleProvider;
            _context = context;
            _statechart = statechart;
            _defaultLevel = defaultLevel;
        }

        #region IPermissionProvider Members

        /// <summary>
        /// Returns UIPermissionLevel for given UI element name
        /// </summary>
        /// <param name="elementName">UI element name</param>
        /// <returns>UI permission level</returns>
        public UIPermissionLevel Demand(string elementName)
        {
            State state = _statechart.GetState(_entity.CurrentState);
            foreach (UIElement e in state.UIPermissions)
            {
                if (e.Name == elementName)
                {
                    UIPermissionLevel res = UIPermissionLevel.Hidden;
                    foreach (Permission p in e.Permissions)
                    {
                        if (p.Role == "*") res = CalcPermisson(res, p.Level);
                        else if (_roleProvider.IsInRole(p.Role, _entity.Entity, _context))
                            res = CalcPermisson(res, p.Level);
                    }
                    return res;
                }
            }
            return _defaultLevel;
        }

        #endregion

        private UIPermissionLevel CalcPermisson(UIPermissionLevel l1, UIPermissionLevel l2)
        {
            return l1 < l2 ? l2 : l1;
        }
    }
}
