using System;
using System.Configuration;
using khi.Catalog;

namespace khi
{
    public sealed class Configuration
    {
        private static readonly System.Configuration.Configuration Config =
            ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        public static bool LoadSongUpdatesOnStartup
        {
            get => GetBool("LoadSongUpdatesOnStartup", false);
            set => Set("LoadSongUpdatesOnStartup", value);
        }

        public static decimal Retries
        {
            get => GetDecimal("Retries", 1);
            set => Set("Retries", value);
        }

        public static bool OverwriteIfFileExists
        {
            get => GetBool("OverwriteIfFileExists", true);
            set => Set("OverwriteIfFileExists", value);
        }

        public static string LastUpdateAlbumList
        {
            get => GetString("LastUpdateAlbumList", null);
            set => Set("LastUpdateAlbumList", value);
        }

        public static string LastSelectedFolder
        {
            get => GetString("LastSelectedFolder", null);
            set => Set("LastSelectedFolder", value);
        }

        public static string PreferredFileType
        {
            get => GetString("PreferredFileType", FileType.MP3.ToString());
            set => Set("PreferredFileType", value);
        }
        
        private static void Set(string key, object value)
        {
            var valueAsString = value.ToString();

            var element = Config.AppSettings.Settings[key];
            if (element == null)
                Config.AppSettings.Settings.Add(new KeyValueConfigurationElement(key, valueAsString));
            else
                element.Value = valueAsString;
        }

        private static bool GetBool(string key, bool defaultValue)
        {
            return GetOrDefault(key, defaultValue, bool.Parse);
        }

        private static string GetString(string key, string defaultValue)
        {
            return GetOrDefault(key, defaultValue, str => str);
        }

        private static decimal GetDecimal(string key, decimal defaultValue)
        {
            return GetOrDefault(key, defaultValue, decimal.Parse);
        }

        private static T GetOrDefault<T>(string key, T defaultValue, Func<string, T> converter)
        {
            var value = Config.AppSettings.Settings[key];
            return value == null ? defaultValue : converter(value.Value);
        }

        public static void Save()
        {
            Config.Save(ConfigurationSaveMode.Modified);
        }
    }
}