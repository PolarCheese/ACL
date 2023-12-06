using System.Text.Json;

namespace ACL.Save
{
    public class SaveManager
    {   
        public static string GetGamePath(string name) => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name);

        private static JsonSerializerOptions serializationOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        };
        public static void SaveFile(string name, string content)
        {
            // Save text as file.
            File.WriteAllText(GetGamePath(name), content);
        }
        public static void LoadFile(string path)
        {
            // Read text from file.
            File.ReadAllText(GetGamePath(path));
        }
        public static void SaveJSON<T>(string name, T json)
        {
            // Create/Save a json file.
            string jsonPath = GetGamePath(name);
            string jsonString = JsonSerializer.Serialize(json , serializationOptions);
            File.WriteAllText(jsonPath, jsonString);
        }
        public static T LoadJson<T>(string name) where T : new()
        {
            T? json;
            string jsonPath = GetGamePath(name);

            if (File.Exists(jsonPath))
            {
                json = JsonSerializer.Deserialize<T>(File.ReadAllText(jsonPath), serializationOptions);
            }
            else
            {
                json = new T();
            }
            return json;
    }
    
    }
}