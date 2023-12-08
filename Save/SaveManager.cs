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
        #region JSON
        public static void SaveJSON<T>(string name, T json, JsonSerializerOptions? jsonSerializerOptions = null)
        {
            // Create/Save a json file.
            string jsonPath = GetGamePath(name);
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
        public static T? LoadJson<T>(string name, JsonSerializerOptions? jsonSerializerOptions = null) where T : new()
        {
            T? json;
            string jsonPath = GetGamePath(name);

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

        public static T? EnsureJson<T>(string name, JsonSerializerOptions? jsonSerializerOptions = null) where T : new()
        {
            T? json;
            string jsonPath = GetGamePath(name);

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