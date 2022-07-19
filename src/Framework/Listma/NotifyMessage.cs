using System;

namespace Listma
{
    /// <summary>
    /// Represents the method that will handle send notification message event 
    /// </summary>
    /// <param name="message">sending message</param>
    public delegate void SendMessageEventHandler(NotifyMessage message);
    
    /// <summary>
    /// State transition notification message
    /// </summary>
    public class NotifyMessage
    {
        string _to;
        string _cc;
        string _subject;
        string _body;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="to">"to" address</param>
        /// <param name="cc">"cc" address</param>
        /// <param name="subj">subject</param>
        /// <param name="body">body</param>
        public NotifyMessage(string to, string cc, string subj, string body)
        {
            if (to.IsNullOrEmpty())
                throw new ArgumentNullException("to");
            _to = to;
            _cc = cc;
            _subject = subj;
            _body = body;
        }

        /// <summary>
        /// To address string 
        /// </summary>
        public string To
        {
            get { return _to; }
            set { _to = value; }
        }

        /// <summary>
        /// CC address string
        /// </summary>
        public string Cc
        {
            get { return _cc; }
            set { _cc = value; }
        }

        /// <summary>
        /// Message subject
        /// </summary>
        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        /// <summary>
        /// Message body
        /// </summary>
        public string Body
        {
            get { return _body; }
            set { _body = value; }
        }

    }
}
