using System.Configuration;

namespace WMPlayer.Web.Models
{
    public static class Settings
    {
        public static string MediaPath { get { return ConfigurationManager.AppSettings["MediaPath"]; } }
        public static string LibraryFilePath { get { return ConfigurationManager.AppSettings["LibraryFilePath"]; } }
    }
}