using System.Text.Json;

namespace ACL.IO;

public static class IOManager
{   
    public static string GetGamePath(string name)
    {
        // File name checks
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("File name cannot be empty.", nameof(name));
        if (VerifyFileName(name)) throw new ArgumentException("File name cannot include invalid characters.", nameof(name));

        // Path checks
        string gamePath = AppDomain.CurrentDomain.BaseDirectory;
        string fullPath = Path.Combine(gamePath, name);
        string fullPathNormalized = Path.GetFullPath(fullPath);
        if (!fullPathNormalized.StartsWith(gamePath, StringComparison.OrdinalIgnoreCase))
            throw new UnauthorizedAccessException("Access to the specified path is denied.");

        return fullPath;
    }

    public static bool VerifyFileName(string name)
    {
        // Checks if name contains any invalid characters
        char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
        return !name.Any(c => invalidFileNameChars.Contains(c));
    }

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
        if (File.Exists(GetGamePath(path))) return true;
        return false;
    }

    public static string ListFiles(string path) // Returns a string listing every file in a given path.
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
    public static string ListAll(string path) // Returns a string listing every directory and file in a given path.
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
    public static void CreateDirectory(string path)
    {
        // Create a directory.
        Directory.CreateDirectory(GetGamePath(path));
    }
    public static void RemoveDirectory(string path)
    {
        // Delete a directory. Only works on empty directories.
        if (CheckDirectory(path)) Directory.Delete(path);
    }
    public static bool CheckDirectory(string path)
    {
        // Check for a directory.
        if (Directory.Exists(GetGamePath(path))) return true;
        return false;
    }
    public static string ListDirectories(string path) // Returns a string listing every directory in a given path.
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
    public static readonly JsonSerializerOptions DefaultOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
    };
    public static void SaveJSON<T>(string path, T JSON, JsonSerializerOptions? options = null)
    {
        options ??= DefaultOptions;

        string JSONPath = GetGamePath(path);
        string JSONData = JsonSerializer.Serialize(JSON, options);
        File.WriteAllText(JSONPath, JSONData);
    }

    public static T LoadJSON<T>(string path, JsonSerializerOptions? options = null) where T : new()
    {
        options ??= DefaultOptions;

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