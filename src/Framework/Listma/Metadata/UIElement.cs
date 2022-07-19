using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Listma
{
    /// <summary>
    /// UI element permissions
    /// </summary>
    [XmlType(Namespace = Statechart.XmlNamespace)]
    public class UIElement
    {
        /// <summary>
        /// UI element name
        /// </summary>
        [XmlAttribute]
        public string Name = string.Empty;

        List<Permission> _permissions;
        
        /// <summary>
        /// Permissions list
        /// </summary>
        [XmlElement("Permission")]
        public Permission[] Permissions
        {
            get
            {
                if(_permissions == null)
                    _permissions = new List<Permission>();
                return _permissions.ToArray();
            }
            set
            {
                if (value != null) _permissions = new List<Permission>(value);
                else _permissions = new List<Permission>();
            }
        }

        /// <summary>
        /// Adds permission
        /// </summary>
        /// <param name="role"></param>
        /// <param name="level"></param>
        public void AddPermission(string role, UIPermissionLevel level)
        {
            if (GetPermission(role) != null)
                throw new WorkflowException("Error on add permission for UI element. Permission for role '{0}' for UI element '{1}' already exists.", role, this.Name);
            Permission p = new Permission();
            p.Role = role;
            p.Level = level;
            _permissions.Add(p);
        }

        /// <summary>
        /// Gets permission by role name
        /// </summary>
        /// <param name="role">role name</param>
        /// <returns>permission</returns>
        public Permission GetPermission(string role)
        {
            foreach (Permission p in Permissions)
                if (p.Role == role) return p;
            return null;
        }
    }
}
