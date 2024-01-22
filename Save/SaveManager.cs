using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace ACL.Save
{
    public class SaveManager
    {   
        public static string GetGamePath(string name) => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name);

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
        // JSON Serializer
        public static JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        };

        public static void SaveJSON<T>(string path, T JSON)
        {
            string JSONPath = GetGamePath(path);
            string JSONData = JsonSerializer.Serialize(JSON, options);
            File.WriteAllText(JSONPath, JSONData);
        }

        public static T LoadJSON<T>(string path) where T : new()
        {
            T JSON;
            string JSONPath = GetGamePath(path);
            if (File.Exists(JSONPath))
            {
                JSON = JsonSerializer.Deserialize<T>(File.ReadAllText(JSONPath), options);
            }
            else
            {
                JSON = new T();
                string jsonString = JsonSerializer.Serialize(JSON, options);
                File.WriteAllText(JSONPath, jsonString);
            }

            return JSON;
        }
        #endregion
    }
}