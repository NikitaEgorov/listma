using System;

namespace Listma
{
    /// <summary>
    /// Provides interface of securable UI view
    /// </summary>
    public interface ISecurable
    {
        /// <summary>
        /// Accepts defined permissions to all UI elements
        /// </summary>
        /// <param name="provider">IPermissionProvider provides UI elements permissions for current user</param>
        void AcceptPermissions(IPermissionProvider provider);
    }
}
