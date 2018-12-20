using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicPortal.Infrastructure
{
    public static class AppConstants
    {
        public static readonly string SONGS_FOLDER_VIRTUAL_PATH = @"~/Content/Songs/";
        public static int PASSOWORD_SALT_LENGTH = 16;
    }
}