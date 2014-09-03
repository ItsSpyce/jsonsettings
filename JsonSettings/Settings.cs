using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace JsonSettings
{
    public class Settings
    {
        /// <summary>
        /// The settings class fetches the settings.json from the local installation folder.
        /// It only reads the file on startup and writes on close. 
        /// If you plan on editing the Settings file, edit '_settings' directly.
        /// </summary>
        private static string SettingsFile { get; set; }
        private static JObject _settings = new JObject();

        /// <summary>
        /// Initializes a new instance of a settings file at requested location.
        /// </summary>
        /// <param name="location">Location of the file.</param>
        public Settings(string location)
        {
            SettingsFile = location;
            _settings = new JObject(SettingsObject);
            Initialize();
        }

        public string this[string key]
        {
            get
            {
                if (_settings[key] != null) return (String)_settings[key];
                _settings.Add(key, key);
                return (String)_settings[key];
            }
            set
            {
                ChangeSetting(key, value);
            }
        }

        /// <summary>
        /// Creates the .JSON file if it doesn't exist already.
        /// </summary>
        private static void Initialize()
        {
            if (File.Exists(SettingsFile)) return;
            var jo = new JObject();
            jo["settings"] = new JObject();
            File.WriteAllText(SettingsFile, jo.ToString());
        }

        /// <summary>
        /// Adds the specified key to the settings JObject.
        /// </summary>
        /// <param name="tokenKey"></param>
        /// <param name="value"></param>
        public void AddSetting(string tokenKey, string value)
        {
            _settings.Add(tokenKey, value);
        }

        /// <summary>
        /// Adds the specified bool value to the settings JObject.
        /// </summary>
        /// <param name="tokenKey"></param>
        /// <param name="value"></param>
        public void AddSetting(string tokenKey, bool value)
        {
            _settings.Add(tokenKey, value);
        }

        /// <summary>
        /// Changes the string value of the specified key.
        /// If the key doesn't exist, it will create it.
        /// </summary>
        /// <param name="tokenKey"></param>
        /// <param name="value"></param>
        public void ChangeSetting(string tokenKey, string value)
        {
            if (_settings[tokenKey] != null)
            {
                _settings[tokenKey].Replace(value);
                return;
            }
            _settings.Add(tokenKey, value);
        }

        /// <summary>
        /// Changes the bool value of the specified key.
        /// If the key doesn't exist, it will create it.
        /// </summary>
        /// <param name="tokenKey"></param>
        /// <param name="value"></param>
        public void ChangeSetting(string tokenKey, bool value)
        {
            if (_settings[tokenKey] != null)
            {
                _settings[tokenKey].Replace(value);
                return;
            }
            _settings.Add(tokenKey, value);
        }

        /// <summary>
        /// Removes the setting from the settings JObject.
        /// </summary>
        /// <param name="tokenKey"></param>
        public void RemoveSetting(string tokenKey)
        {
            _settings[tokenKey].Remove();
        }

        /// <summary>
        /// Saves the settings JObject to the file.
        /// </summary>
        public void Save()
        {
            var saveObject = new JObject();
            saveObject["settings"] = _settings ?? new JObject();
            File.WriteAllText(SettingsFile, saveObject.ToString());
        }

        /// <summary>
        /// Clear the previous settings file to default.
        /// </summary>
        public void Clear()
        {
            _settings.RemoveAll();
            Save();
        }

        /// <summary>
        /// Fetches the text from the JSON file and parses it to 
        /// the settings JObject.
        /// </summary>
        private static JObject SettingsObject
        {
            get
            {
                var lines = File.ReadAllLines(SettingsFile);
                var line = "";

                //begins scanning lines
                foreach (var l in lines.Where(l => !l.StartsWith("//")))
                {
                    var i = 0;
                    var isReadingComment = false;
                    var passing = "";

                    //begins scanning line for commented string. 
                    foreach (var c in l)
                    {
                        if (c == '/' && l[i++] == '*' && !isReadingComment) isReadingComment = true;
                        if (c == '*' && l[i++] == '/' && isReadingComment) isReadingComment = false;
                        if (!isReadingComment) passing += c;
                        i++;
                    }
                    line += passing;
                }

                var o = JObject.Parse(line);
                return (JObject)o["settings"];
            }
        }
    }
}
