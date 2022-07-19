using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Listma;
using BugTracker.Domain;

namespace BugTracker.workflow
{
    public class BugHandlerBase : IHandler<Issue, DBContext>
    {
        virtual protected string Result { get { return String.Empty; } }

        #region IHandler<Issue,DBContext> Members

        public void Execute(Issue entity, DBContext context)
        {
            entity.Result = Result;
        }

        #endregion
    }

    public class FixHandler : BugHandlerBase
    {
        override protected string Result { get { return "Fixed"; } }
    }

    public class FADHandler : BugHandlerBase
    {
        override protected string Result { get { return "Functions as designed"; } }
    }

    public class DuplicateHandler : BugHandlerBase
    {
        override protected string Result { get { return "Duplicate"; } }
    }

    public class WontFixHandler : BugHandlerBase
    {
        override protected string Result { get { return "Wont fix"; } }
    }

    public class DontReproduceHandler : BugHandlerBase
    {
        override protected string Result { get { return "Don't reproduce"; } }
    }

    public class ReopenHandler : BugHandlerBase
    {
        override protected string Result { get { return "Reopen"; } }
    }

    public class AssignHandler : BugHandlerBase, ITransitionHandler<Issue, DBContext>
    {

        #region ITransitionHandler<Issue,DBContext> Members

        public void PreValidate(Issue entity)
        {
            if (!entity.AssignedToReference.IsLoaded) entity.AssignedToReference.Load();
            if(entity.AssignedTo == null) throw new ApplicationException("'Assigned To' property must be set.");
        }

        public bool ConfirmStateChange(Issue entity, string targetState)
        {
            return true;
        }

        #endregion
    }
}
