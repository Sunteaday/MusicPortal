using MusicPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicPortal.Infrastructure
{
    public struct MenuItem
    {
        public string Action { get; set; }

        public string Controller { get; set; }

        public string VisibleName { get; set; }
    }

    public static class MenuProvider
    {
        public static readonly MenuItem index = new MenuItem()
        {
            Action = "Index",
            Controller = "Songs",
            VisibleName = "Index"
        };
        public static readonly MenuItem login = new MenuItem()
        {
            Action = "Login",
            Controller = "User",
            VisibleName = "Login"
        };
        public static readonly MenuItem logout = new MenuItem()
        {
            Action = "Logout",
            Controller = "User",
            VisibleName = "Logout"
        };
        public static readonly MenuItem register = new MenuItem()
        {
            Action = "Register",
            Controller = "User",
            VisibleName = "Register"
        };
        public static readonly MenuItem AdminPanel = new MenuItem()
        {
            Action = "Index",
            Controller = "AdminPanel",
            VisibleName = "Admin Panel"
        };
        public static readonly MenuItem uploadMusic = new MenuItem()
        {
            Action = "Create",
            Controller = "Songs",
            VisibleName = "Upload Song"
        };

        public static IEnumerable<MenuItem> BuildMenu(IEnumerable<string> userRoles)
        {
            List<MenuItem> userMenu = new List<MenuItem>();

            userRoles = userRoles ?? new List<string>() { String.Empty };
            foreach (string status in userRoles)
            {
                switch (status)
                {
                    case "Admin":
                        {
                            userMenu.Add(AdminPanel);
                            break;
                        }
                    case "Authorized":
                        {
                            userMenu.Add(index);
                            userMenu.Add(uploadMusic);
                            userMenu.Add(logout);
                            break;
                        };
                    case "Authenticated":
                        {
                            userMenu.Add(index);
                            userMenu.Add(logout);
                            break;
                        };
                    default:
                        {
                            userMenu.Add(index);
                            userMenu.Add(login);
                            userMenu.Add(register);
                            break;
                        }
                }
            }
            return userMenu;
        }
    }
}