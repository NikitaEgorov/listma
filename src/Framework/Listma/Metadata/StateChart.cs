using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Collections;

namespace Listma
{
    /// <summary>
    /// Statechart
    /// </summary>
    [XmlRoot(Namespace = Statechart.XmlNamespace)]
    public class Statechart
    {
        /// <summary>
        /// Xml namespace
        /// </summary>
        public const string XmlNamespace = "urn:Listma:Statechart";
        
        /// <summary>
        /// Statechart Id
        /// </summary>
        [XmlAttribute]
        public string Id = string.Empty;

        List<State> _states;

        /// <summary>
        /// States list
        /// </summary>
        [XmlElement("State")]
        public State[] States
        {
            get
            {
                if (_states == null)
                    _states = new List<State> ();
                return _states.ToArray();
            }
            set
            {
                if (value != null) _states = new List<State>(value);
                else _states = new List<State>();
            }
        }

        List<NotifyTemplate> _notifyTemplates;

        /// <summary>
        /// Notification message templates list
        /// </summary>
        [XmlArray]
        public NotifyTemplate[] NotifyTemplates
        {
            get 
            {
                if (_notifyTemplates == null)
                    _notifyTemplates = new List<NotifyTemplate>();
                return _notifyTemplates.ToArray(); 
            }
            set 
            {
                if (value != null) _notifyTemplates = new List<NotifyTemplate>(value);
                else _notifyTemplates = new List<NotifyTemplate>();
            }
        }

        internal NotifyTemplate GetNotifyTemplate(string id)
        {
            foreach (NotifyTemplate n in NotifyTemplates)
                if (n.Id == id) return n;
            return null;
        }

        internal State GetState(string id)
        {
            foreach (State s in States)
                if (s.Id == id) return s;
            return null;
        }

        internal IEnumerable<State> GetStates(bool initialOnly)
        {
            foreach (State s in States)
                if (!initialOnly | (s.Initial & initialOnly)) yield return s;
        }

        internal void AddState(State state)
        {
            if (GetState(state.Id) != null)
                throw new WorkflowException("Error on add state into statechart. State '{0}' already exists in the statechart.", state.Id);
            _states.Add(state);
        }

        internal void AddNotifyTemplate(string templateId, string subjectTemplate, string bodyTemplate)
        {
            if(GetNotifyTemplate(templateId) != null)
                throw new WorkflowException("Error on add notify template into statechart. Notify template '{0}' already exists in the statechart.", templateId);
            NotifyTemplate nt = new NotifyTemplate();
            nt.Id = templateId;
            nt.Subject = subjectTemplate;
            nt.Body = bodyTemplate;
            _notifyTemplates.Add(nt);
        }
    }
}
