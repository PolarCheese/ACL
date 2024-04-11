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

        public string ListFiles(string path) // Returns a string listing every file in a given path.
        {
            string List = string.Empty, gamePath = GetGamePath(path);
            if (CheckDirectory(path)) // Check for directory.
            {
                foreach (var file in Directory.EnumerateFiles(gamePath))
                {
                    var shortFile = file.Split("/").Last();
                    List += $"{shortFile} ";
                }
            }
            return List;
        }
        public string ListAll(string path) // Returns a string listing every directory and file in a given path.
        {
            string List = string.Empty, gamePath = GetGamePath(path);
            if (CheckDirectory(path)) // Check for directory.
            {
                foreach (string Entry in Directory.EnumerateFileSystemEntries(gamePath))
                {
                    var shortDir = Entry.Split("/").Last();
                    List += $"{shortDir} ";
                }
            }
            return List;
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
        public string ListDirectories(string path) // Returns a string listing every directory in a given path.
        {
            string List = string.Empty, gamePath = GetGamePath(path);
            if (CheckDirectory(path)) // Check for directory.
            {
                foreach (string directory in Directory.EnumerateDirectories(gamePath))
                {
                    var shortDir = directory.Split("/").Last();
                    List += $"{shortDir} ";
                }
            }
            return List;
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