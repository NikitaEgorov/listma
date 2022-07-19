using System;
using Listma;
using BugTracker.Domain;
using BugTracker.BL;

namespace BugTracker.Workflow
{
    public class IssueRoleProvider : IRoleProvider<Domain.Issue, DBContext>
    {
        #region IRoleProvider Members

        public bool IsInRole(string roleName, Domain.Issue entity, DBContext context)
        {
            string userName = System.Threading.Thread.CurrentPrincipal.Identity.Name;
            bool result = false;
            if(string.IsNullOrEmpty(userName)) return result;
            switch (roleName)
            {
                case "Owner":
                    if (!entity.OwnerReference.IsLoaded) entity.OwnerReference.Load();
                    result = userName == entity.Owner.Name;
                    break;
                case "Performer":
                    if (!entity.AssignedToReference.IsLoaded) entity.AssignedToReference.Load();
                    if (entity.AssignedTo != null) result = userName == entity.AssignedTo.Name;
                    break;
                default:
                    if (!entity.ProjectReference.IsLoaded) entity.ProjectReference.Load();
                    result = UserService.IsUserInRole(userName, roleName, entity.Project.Id, context);
                    break;
            }
            return result;
        }

        #endregion
    }
}