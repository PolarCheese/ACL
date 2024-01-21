using System.Text.Json;

namespace ACL.Save
{
    public class SaveManager
    {   
        public static string GetGamePath(string name) => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name);

        private static JsonSerializerOptions defaultSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        };
        #region Files
        public static void SaveFile(string path, string content)
        {
            // Save text as file.
            File.WriteAllText(GetGamePath(path), content);
        }
        public static void RemoveFile(string path)
        {
            // Remove a file.
            File.Delete(path);
        }
        public static string LoadFile(string path)
        {
            // Read text from file.
            return File.ReadAllText(GetGamePath(path));
        }
        public static bool CheckFile(string path)
        {
            // Check for a file.
            if (File.Exists(GetGamePath(path))) { return true; }
            return false;
        }
        #endregion
        #region Directories
        public static void CreateDirectory(string path)
        {
            // Create a directory.
            Directory.CreateDirectory(GetGamePath(path));
        }
        public static void RemoveDirectory(string path)
        {
            // Delete a directory. Only works on empty directories.
            if (CheckDirectory(path)) { Directory.Delete(path); }
        }
        public static bool CheckDirectory(string path)
        {
            // Check for a directory.
            if (Directory.Exists(GetGamePath(path))) { return true; }
            return false;
        }
        #endregion
        #region JSON
        public static void SaveJson<T>(string path, T json, JsonSerializerOptions? jsonSerializerOptions = null)
        {
            // Create/Save a json file.
            string jsonPath = GetGamePath(path);
            if (jsonSerializerOptions == null)
            {
                string jsonString = JsonSerializer.Serialize(json , defaultSerializerOptions);
                File.WriteAllText(jsonPath, jsonString);
            }
            else
            {
                string jsonString = JsonSerializer.Serialize(json, jsonSerializerOptions);
                File.WriteAllText(jsonPath, jsonString);
            }
        }
        public static T? LoadJson<T>(string path, JsonSerializerOptions? jsonSerializerOptions = null) where T : new()
        {
            T? json;
            string jsonPath = GetGamePath(path);

            if (File.Exists(jsonPath))
            {
                if (jsonSerializerOptions == null)
                {
                    json = JsonSerializer.Deserialize<T>(File.ReadAllText(jsonPath), defaultSerializerOptions);
                }
                else
                {
                    json = JsonSerializer.Deserialize<T>(File.ReadAllText(jsonPath), jsonSerializerOptions);
                }
            }
            else
            {
                json = new T();
            }
            return json;
        }

        public static T? EnsureJson<T>(string path, JsonSerializerOptions? jsonSerializerOptions = null) where T : new()
        {
            T? json;
            string jsonPath = GetGamePath(path);

            if (File.Exists(jsonPath))
            {
                if (jsonSerializerOptions == null)
                {
                    json = JsonSerializer.Deserialize<T>(File.ReadAllText(jsonPath), defaultSerializerOptions);
                }
                else
                {
                    json = JsonSerializer.Deserialize<T>(File.ReadAllText(jsonPath), jsonSerializerOptions);
                }
            }
            else
            {
                json = new T();
                string jsonString = JsonSerializer.Serialize(json, jsonSerializerOptions);
                File.WriteAllText(jsonPath, jsonString);
            }

            return json;
        }
        #endregion
    }
}