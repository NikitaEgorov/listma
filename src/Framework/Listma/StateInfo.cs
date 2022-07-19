using System;

using Listma;

namespace Listma
{
    /// <summary>
    /// State info class
    /// </summary>
    public class StateInfo
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StateInfo() { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        public StateInfo(string id) 
        {
            _Id = id;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="state"></param>
        public StateInfo(State state)
        {
            _Id = state.Id;
            _Title = state.Title;
        }

        readonly string _Id = string.Empty;
        /// <summary>
        /// Gets state Id
        /// </summary>
        public string Id { get { return _Id; } }
        readonly string _Title = string.Empty;
        /// <summary>
        /// Gets state Title
        /// </summary>
        public string Title { get { return _Title; } }
    }
}
