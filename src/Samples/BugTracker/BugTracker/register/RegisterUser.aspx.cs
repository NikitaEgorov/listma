using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BugTracker.BL;
using BugTracker.Domain;
using System.Web.Security;
using System.Security.Principal;

namespace BugTracker
{
    public partial class RegisterUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

       
        protected void Button1_Click(object sender, EventArgs e)
        {
            User user = UserService.RegisterUser(txtName.Text, txtPassword.Text, "");
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(user.Name,
                false, 15);
            FormsIdentity identity = new FormsIdentity(ticket);
            GenericPrincipal principal = new GenericPrincipal(identity, new string[] { });
            Context.User = principal;
            FormsAuthentication.SetAuthCookie(user.Name, false);
            Response.Redirect("/");
        }

      
    }
}
