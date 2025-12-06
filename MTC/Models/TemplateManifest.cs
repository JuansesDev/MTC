namespace MTC.Models;

public class TemplateManifest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Version { get; set; } = "1.0.0";
    public string Author { get; set; } = string.Empty;
    public string[] Tags { get; set; } = Array.Empty<string>();
    public Dictionary<string, string> Variables { get; set; } = new();
}
