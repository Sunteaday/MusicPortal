using MusicPortal.Models;
using MusicPortal.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicPortal.Infrastructure
{
    public static class Extensions
    {
        public static SongVM ToViewModel(this Song song)
        {
            return new SongVM()
            {
                Id = song.Id,
                Name = song.Name,
                Genres = song.Genres.Select(genre =>
                 new SelectedGenre()
                 {
                     Id = genre.Id,
                     Name = genre.Name,
                     IsSelected = true
                 })
            };
        }

        public static UserVM ToViewModel(this User u)
        {
            return new UserVM()
            {
                Id = u.Id,
                Login = u.Login,
                Roles = u.Roles.Select(role =>
                    new SelectedRole()
                    {
                        Id = role.Id,
                        Name = role.Name,
                        IsSelected = true
                    })
            };
        }

        public static UserVM ToViewModel(this User u, IEnumerable<UserRole> allRoles)
        {
            return new UserVM()
            {
                Id = u.Id,
                Login = u.Login,
                Roles = allRoles.Select(dbRole =>
                    new SelectedRole()
                    {
                        Id = dbRole.Id,
                        Name = dbRole.Name,
                        IsSelected = u.Roles.FirstOrDefault(r => r.Id == dbRole.Id) != null
                    })
            };
        }

        public static SelectedGenre ToViewModel(this Genre g)
        {
            return new SelectedGenre()
            {
                Id = g.Id,
                Name = g.Name,
                IsSelected = false
            };
        }

        // no Identity
        public static bool UserHasRole(this HttpSessionStateBase Session, string role)
        {
            if (!(Session["Roles"] is IEnumerable<string> userRoles))
                return false;

            return userRoles.Contains(role);
        }

        public static string GetVirtualFilePath(this Song song)
        {
            return Path.Combine(AppConstants.SONGS_FOLDER_VIRTUAL_PATH, song.FileName);
        }

    }
}