using System;


namespace Listma
{
    /// <summary>
    /// The exception that is thrown when a non-fatal workflow error occurs
    /// </summary>
    [Serializable]
    public class WorkflowException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the WorkflowException class
        /// </summary>
        public WorkflowException() : base() { }
        /// <summary>
        /// Initializes a new instance of the WorkflowException class with a specified error message.
        /// </summary>
        /// <param name="message"></param>
        public WorkflowException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the WorkflowException class with a specified error message and inner exception.
        /// </summary>
        /// <param name="message">error message</param>
        /// <param name="inner">inner exception</param>
        public WorkflowException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Initializes a new instance of the WorkflowException class with a error message template.
        /// </summary>
        /// <param name="template">message template</param>
        /// <param name="args">template parameters</param>
        public WorkflowException(string template, params object[] args) : base(String.Format(template, args)) { }
        /// <summary>
        /// Initializes a new instance of the WorkflowException class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected WorkflowException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
