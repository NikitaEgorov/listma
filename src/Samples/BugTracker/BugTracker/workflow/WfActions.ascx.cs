using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BugTracker.BL;

namespace BugTracker.workflow
{
    public partial class WfActions : System.Web.UI.UserControl
    {
        public event CommandEventHandler DoCommand;

        
        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if(DoCommand != null) DoCommand(source, e);
        }
    }
}