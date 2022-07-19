using System;
using Listma;

namespace Listma
{
    /// <summary>
    /// State transition info class
    /// </summary>
    public class TransitionInfo
    {
        /// <summary>
        /// Constructor 
        /// </summary>
        public TransitionInfo() { }
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="t">state transition</param>
        public TransitionInfo(Transition t) 
        {
            _Id = t.Id;
            _Title = t.Title;
            _TargetState = t.TargetState;
        }

        readonly string _Id = string.Empty;
        /// <summary>
        /// Gets transition Id
        /// </summary>
        public string Id { get { return _Id; } }
        readonly string _Title = string.Empty;
        /// <summary>
        /// Gets transition Title
        /// </summary>
        public string Title { get { return _Title; } }
        readonly string _TargetState = string.Empty;
        /// <summary>
        /// Gets transition's target state
        /// </summary>
        public string TargetState { get { return _TargetState; } }
    }
}
