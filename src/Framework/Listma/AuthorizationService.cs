using System;


namespace Listma
{
    internal static class AuthorizationService
    {
        internal static bool Authorize<T, C>(IWorkflowAdapter<T> entity, Transition t, IConfigProvider _config, C context)
        {
            if (t.Performers.Length == 0) return true;
            IRoleProvider<T,C> roleProvider = _config.GetRoleProvider<T,C>(entity.EntityType);
            foreach (Performer p in t.Performers)
                if (roleProvider.IsInRole(p.Role, entity.Entity, context)) return true;
            return false;
        }
    }
}
