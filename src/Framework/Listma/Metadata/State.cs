using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Listma
{
    /// <summary>
    /// State
    /// </summary>
    [XmlType(Namespace = Statechart.XmlNamespace)]
    public class State
    {
        /// <summary>
        /// State Id
        /// </summary>
        [XmlAttribute]
        public string Id = string.Empty;
        /// <summary>
        /// State title
        /// </summary>
        [XmlAttribute]
        public string Title = string.Empty;
        /// <summary>
        /// Can state to be initial
        /// </summary>
        [XmlAttribute]
        public bool Initial = true;
        /// <summary>
        /// OnStateEnter event handler class name
        /// </summary>
        [XmlAttribute]
        public string OnEnterHandler = string.Empty;
        /// OnStateExit event handler class name
        [XmlAttribute]
        public string OnExitHandler = string.Empty;
        /// <summary>
        /// State description
        /// </summary>
        [XmlElement]
        public Description Description = new Description();

        /// <summary>
        /// Runtime OnStateEnter event handler class
        /// </summary>
        [XmlIgnore]
        public object RuntimeEnterHandler;

        /// <summary>
        /// Runtime OnStateExit event handler class
        /// </summary>
        [XmlIgnore]
        public object RuntimeExitHandler;

        List<Transition> _transitions = null;

        /// <summary>
        /// State transitions list
        /// </summary>
        [XmlElement("Transition")]
        public Transition[] Transitions
        {
            get
            {
                if (_transitions == null)
                    _transitions = new List<Transition>();
                return _transitions.ToArray();
            }
            set
            {
                if(value != null)
                    _transitions = new List<Transition>(value);
            }
        }

        List<UIElement> _UIPermissions;

        /// <summary>
        /// UI elements permissions list
        /// </summary>
        [XmlArray]
        public UIElement[] UIPermissions
        {
            get
            {
                if (_UIPermissions == null)
                    _UIPermissions =  new List<UIElement>();
                return _UIPermissions.ToArray();
            }
            set
            {
                if (value != null) _UIPermissions = new List<UIElement>(value);
                else _UIPermissions = new List<UIElement>();
            }
        }

        internal Transition GetTransition(string transitionId)
        {
            foreach (Transition t in Transitions)
                if (t.Id == transitionId) return t;
            throw new WorkflowException("Transition '{0}' is not exist.", transitionId);
        }

        internal Transition FindTransition(string transitionId)
        {
            foreach (Transition t in Transitions)
                if (t.Id == transitionId) return t;
            return null;
        }

        internal void AddTransition(Transition t)
        {
            if (FindTransition(t.Id) != null)
                throw new WorkflowException("Error on add transition into state. Transition '{0}' already exists in the state '{1}'.", t.Id, this.Id);
            _transitions.Add(t);
        }

        internal void AddUIElement(UIElement e)
        {
            if (GetUIElement(e.Name) != null)
                throw new WorkflowException("Error on add UI element permissions. Ui element '{0}' already exists.", e.Name);
            _UIPermissions.Add(e);
        }

        internal UIElement GetUIElement(string name)
        {
            foreach (UIElement uie in UIPermissions)
                if (uie.Name == name) return uie;
            return null;
        }
    }
}
