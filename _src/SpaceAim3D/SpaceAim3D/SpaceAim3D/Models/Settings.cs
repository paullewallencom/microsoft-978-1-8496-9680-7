using System;
using System.IO.IsolatedStorage;

namespace SpaceAim3D.Models
{
    /// <summary>The class representing settings used in the game.</summary>
    public class Settings
    {
        private static IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        // Keys for settings
        private const string KEY_KEY = "Key";
        private const string KEY_NAME = "Name";
        private const string KEY_VOLUME = "Volume";
        private const string KEY_VIBRATIONS = "Vibrations";
        private const string KEY_LOCATION = "Location";

        // Default values for settings
        private const string DEFAULT_NAME = "Unknown";
        private const float DEFAULT_VOLUME = 50.0f;
        private const bool DEFAULT_VIBRATIONS = true;
        private const bool DEFAULT_LOCATION = false;

        /// <summary>Gets or sets a key of the player.</summary>
        public static string Key
        {
            get { return GetValue<string>(KEY_KEY); }
            set { SetValue<string>(KEY_KEY, value); }
        }

        /// <summary>Gets or sets a name of the player.</summary>
        public static string Name
        {
            get { return GetValue<string>(KEY_NAME); }
            set { SetValue<string>(KEY_NAME, value); }
        }

        /// <summary>Gets or sets a volume value (in range 0.0-1.0).</summary>
        public static float Volume
        {
            get { return GetValue<float>(KEY_VOLUME); }
            set { SetValue<float>(KEY_VOLUME, value); }
        }

        /// <summary>Gets or sets a value indicating whether vibrations are used.</summary>
        public static bool Vibrations
        {
            get { return GetValue<bool>(KEY_VIBRATIONS); }
            set { SetValue<bool>(KEY_VIBRATIONS, value); }
        }

        /// <summary>Gets or sets a value indicating whether 
        /// current location data should be sent to the web service.</summary>
        public static bool Location
        {
            get { return GetValue<bool>(KEY_LOCATION); }
            set { SetValue<bool>(KEY_LOCATION, value); }
        }

        static Settings()
        {
            SetDefaultValue<string>(KEY_KEY, Guid.NewGuid().ToString("N"));
            SetDefaultValue<string>(KEY_NAME, DEFAULT_NAME);
            SetDefaultValue<float>(KEY_VOLUME, DEFAULT_VOLUME);
            SetDefaultValue<bool>(KEY_VIBRATIONS, DEFAULT_VIBRATIONS);
            SetDefaultValue<bool>(KEY_LOCATION, DEFAULT_LOCATION);
        }

        private static void SetDefaultValue<T>(string key, T value)
        {
            if (!settings.Contains(key))
            {
                settings.Add(key, value);
                settings.Save();
            }
        }

        private static T GetValue<T>(string key)
        {
            T value;
            if (settings.TryGetValue<T>(key, out value))
            {
                return value;
            }
            else
            {
                return default(T);
            }
        }

        private static void SetValue<T>(string key, T value)
        {
            settings[key] = value;
            settings.Save();
        }
    }
}
