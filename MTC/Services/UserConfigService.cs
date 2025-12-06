using System.Text.Json;

namespace MTC.Services;

public class UserConfigService : IUserConfigService
{
    private readonly string _configPath;
    private Dictionary<string, string> _config;

    public UserConfigService()
    {
        var homeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var configDir = Path.Combine(homeDir, ".mtc");
        if (!Directory.Exists(configDir))
        {
            Directory.CreateDirectory(configDir);
        }
        _configPath = Path.Combine(configDir, "config.json");
        _config = LoadConfig();
    }

    private Dictionary<string, string> LoadConfig()
    {
        if (!File.Exists(_configPath))
        {
            return new Dictionary<string, string>();
        }

        try
        {
            var json = File.ReadAllText(_configPath);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
        }
        catch
        {
            return new Dictionary<string, string>();
        }
    }

    private void SaveConfig()
    {
        var json = JsonSerializer.Serialize(_config, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_configPath, json);
    }

    public void Set(string key, string value)
    {
        _config[key] = value;
        SaveConfig();
    }

    public string? Get(string key)
    {
        return _config.TryGetValue(key, out var value) ? value : null;
    }

    public Dictionary<string, string> GetAll()
    {
        return new Dictionary<string, string>(_config);
    }
}
