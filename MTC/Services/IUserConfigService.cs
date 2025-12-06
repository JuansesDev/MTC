namespace MTC.Services;

public interface IUserConfigService
{
    void Set(string key, string value);
    string? Get(string key);
    Dictionary<string, string> GetAll();
}
