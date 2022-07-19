using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Listma
{
    /// <summary>
    /// Transition
    /// </summary>
    [XmlType(Namespace=Statechart.XmlNamespace)]
    public class Transition
    {
        /// <summary>
        /// Id
        /// </summary>
        [XmlAttribute]
        public string Id = string.Empty;
        /// <summary>
        /// Title
        /// </summary>
        [XmlAttribute]
        public string Title = string.Empty;
        /// <summary>
        /// Target state Id
        /// </summary>
        [XmlAttribute]
        public string TargetState = string.Empty;
        /// <summary>
        /// Transition handler class name
        /// </summary>
        [XmlAttribute]
        public string Handler = string.Empty;
        /// <summary>
        /// Transition description
        /// </summary>
        [XmlElement]
        public Description Description = new Description();

        [XmlIgnore]
        internal object RuntimeHandler; 
        
        List<Performer> _performers;

        /// <summary>
        /// Performers List
        /// </summary>
        [XmlArray]
        [XmlArrayItem("Performer")]
        public Performer[] Performers
        {
            get 
            {
                if (_performers == null)
                    _performers = new List<Performer> { };
                return _performers.ToArray();
            }
            set 
            {
                if (value != null) _performers = new List<Performer>(value);
                else _performers = new List<Performer>();
            }
        }
        /// <summary>
        /// Adds performer  
        /// </summary>
        /// <param name="role">role name</param>
        public void AddPerformer(string role)
        {
            if (GetPerformer(role) != null)
                throw new WorkflowException("Error on add transition's performer. Performer role '{0}' already exists in transition '{1}'.", role, this.Id);
            Performer p = new Performer();
            p.Role = role;
            _performers.Add(p);
        }

        private object GetPerformer(string role)
        {
            foreach (Performer p in Performers)
                if (p.Role == role) return p;
            return null;
        }

        List<Notification> _notifications;

        /// <summary>
        /// Notifications list
        /// </summary>
        [XmlArray]
        public Notification[] Notifications
        {
            get 
            {
                if (_notifications == null)
                    _notifications = new List<Notification>();
                return _notifications.ToArray(); 
            }
            set
            {
                if (value != null) _notifications = new List<Notification>(value);
                else _notifications = new List<Notification>();
            }
        }

        internal void AddNotification(Notification n)
        {
            if(GetNotification(n.TemplateId) != null)
                throw new WorkflowException("Error on add transition's notification. Notification '{0}' already exists in transition '{1}'.", n.TemplateId, this.Id);
            _notifications.Add(n);
        }

        internal Notification GetNotification(string templateId)
        {
            foreach (Notification n in Notifications)
                if (n.TemplateId == templateId) return n;
            return null;
        }
    }
}
