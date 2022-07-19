using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Listma
{
    /// <summary>
    /// Provides interface for UI permissions provider
    /// </summary>
    public interface IPermissionProvider
    {
        /// <summary>
        /// Returns UIPermissionLevel for given UI element name
        /// </summary>
        /// <param name="elementName">UI element name</param>
        /// <returns>permission level</returns>
        UIPermissionLevel Demand(string elementName);
    }
}
