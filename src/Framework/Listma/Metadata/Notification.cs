using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Listma
{
    /// <summary>
    /// Transfer notification
    /// </summary>
    [XmlType(Namespace = Statechart.XmlNamespace)]
    public class Notification
    {
        /// <summary>
        /// Notification template Id
        /// </summary>
        [XmlAttribute]
        public string TemplateId = string.Empty;

        /// <summary>
        /// Notification handler class name
        /// </summary>
        [XmlAttribute]
        public string Handler = string.Empty;

        List<Recipient> _to;

        /// <summary>
        /// Main recipients list
        /// </summary>
        [XmlElement("To")]
        public Recipient[] To
        {
            get
            {
                if (_to == null)
                    _to = new List<Recipient>();
                return _to.ToArray();
            }
            set 
            {
                if (value != null) _to = new List<Recipient>(value);
                else _to = new List<Recipient>();
            }
        }

        List<Recipient> _cc;

        /// <summary>
        /// Optional recipients list
        /// </summary>
        [XmlElement("Cc")]
        public Recipient[] Cc
        {
            get
            {
                if (_cc == null)
                    _cc = new List<Recipient>();
                return _cc.ToArray();
            }
            set
            {
                if (value != null) _cc = new List<Recipient>(value);
                else _cc = new List<Recipient>();
            }
        }


        [XmlIgnore]
        internal object RuntimeHandler;

        internal void AddTo(Recipient recipient)
        {
            if (GetTo(recipient.RoleOrAddress) != null)
                throw new WorkflowException("Error on add notification's recipient. Recipient '{0}' already exists in the 'To' list.", recipient.RoleOrAddress);
            _to.Add(recipient);
        }

        internal Recipient GetTo(string roleOrAddress)
        {
            foreach (Recipient r in To)
                if (r.Address == roleOrAddress || r.Role == roleOrAddress) return r;
            return null;
        }

        internal void AddCc(Recipient recipient)
        {
            if (GetCc(recipient.RoleOrAddress) != null)
                throw new WorkflowException("Error on add notification's recipient. Recipient '{0}' already exists in the 'Cc' list.", recipient.RoleOrAddress);
            _cc.Add(recipient);
        }

        internal Recipient GetCc(string roleOrAddress)
        {
            foreach (Recipient r in Cc)
                if (r.Address == roleOrAddress || r.Role == roleOrAddress) return r;
            return null;
        }
    }

    /// <summary>
    /// Notification recipient
    /// </summary>
    [XmlType(Namespace = Statechart.XmlNamespace)]
    public class Recipient
    {
        /// <summary>
        /// Initializes the instance
        /// </summary>
        public Recipient() { }
        /// <summary>
        /// Initializes the instance with role or address
        /// </summary>
        /// <param name="role"></param>
        /// <param name="address"></param>
        public Recipient(string role, string address)
        {
            if (role != null) Role = role;
            if (address != null) Address = address;
        }

        /// <summary>
        /// Recipient role
        /// </summary>
        [XmlAttribute]
        public string Role = string.Empty;
        /// <summary>
        /// Recipient address
        /// </summary>
        [XmlAttribute]
        public string Address = string.Empty;

        [XmlIgnore]
        internal string RoleOrAddress
        {
            get 
            {
                if (!Role.IsNullOrEmpty()) return Role;
                else return Address;
            }
        }
    }
}
