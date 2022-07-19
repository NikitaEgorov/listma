using System;

namespace Listma
{
    /// <summary>
    /// UI element permission level
    /// </summary>
    public enum UIPermissionLevel
    {
        /// <summary>
        /// Element is hidden for user
        /// </summary>
        Hidden = 0,
        /// <summary>
        /// Element is read only for user
        /// </summary>
        Read = 1,
        /// <summary>
        /// Element is accessible
        /// </summary>
        Write = 2
    }
}
