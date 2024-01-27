using System.Text.Json;

namespace ACL.IO
{
    public class FileManager
    {   
        public string GetGamePath(string name) => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name);

        #region Files
        public void SaveFile(string path, string content)
        {
            // Save text as file.
            File.WriteAllText(GetGamePath(path), content);
        }
        public void RemoveFile(string path)
        {
            // Remove a file.
            File.Delete(path);
        }
        public string LoadFile(string path)
        {
            // Read text from file.
            return File.ReadAllText(GetGamePath(path));
        }
        public bool CheckFile(string path)
        {
            // Check for a file.
            if (File.Exists(GetGamePath(path))) { return true; }
            return false;
        }
        #endregion
        #region Directories
        public void CreateDirectory(string path)
        {
            // Create a directory.
            Directory.CreateDirectory(GetGamePath(path));
        }
        public void RemoveDirectory(string path)
        {
            // Delete a directory. Only works on empty directories.
            if (CheckDirectory(path)) { Directory.Delete(path); }
        }
        public bool CheckDirectory(string path)
        {
            // Check for a directory.
            if (Directory.Exists(GetGamePath(path))) { return true; }
            return false;
        }
        #endregion
        #region JSON
        // JSON Serializer
        public JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        };

        public void SaveJSON<T>(string path, T JSON)
        {
            string JSONPath = GetGamePath(path);
            string JSONData = JsonSerializer.Serialize(JSON, options);
            File.WriteAllText(JSONPath, JSONData);
        }

        public T LoadJSON<T>(string path) where T : new()
        {
            T? JSON;
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

            return JSON ?? new T();
        }
        #endregion
    }
}