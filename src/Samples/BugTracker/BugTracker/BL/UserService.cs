using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BugTracker.Domain;
using System.Security.Cryptography;
using System.Text;

namespace BugTracker.BL
{
    public static class UserService
    {
        public static User RegisterUser(string name, string password, string email)
        {
            using (DBContext context = new DBContext())
            {
                User user = new User();
                user.Name = name;
                user.Pwdhash = GetPasswordHash(password);
                user.Email = email;
                context.AddToUser(user);
                context.SaveChanges(true);
                return user;
            }
        }

        
        public static bool IsUserRegistered(string name, string password)
        {
            string pwdhash = GetPasswordHash(password);
            using (DBContext context = new DBContext())
            {
                User user = context.User.Where(u => u.Pwdhash == pwdhash).First();
                if (user != null && user.Name == name) return true;
            }
            return false;
        }

        public static bool IsUserInRole(string userName, string roleName, int projectId, DBContext ctx)
        {
            return null != (from user in ctx.User
                           join team in ctx.ProjectTeam
                           on user.Id equals team.User.Id
                           where (user.Name == userName && team.Role.Name == roleName && team.Project.Id == projectId)
                           select user).FirstOrDefault();
            
        }


        private static string GetPasswordHash(string password)
        {
            SHA256Managed sha = new SHA256Managed();
            return Convert.ToBase64String(
                sha.ComputeHash(
                    UnicodeEncoding.Unicode.GetBytes(password)));
        }



    }
}
