using System.Text.Json;
using MTC.Models;

namespace MTC.Services;

public class TemplateService : ITemplateService
{
    private readonly string _templatesPath;

    public TemplateService(string? customPath = null)
    {
        if (!string.IsNullOrEmpty(customPath))
        {
            _templatesPath = customPath;
            return;
        }

        // Search order:
        // 1. "templates" folder in current execution directory (Portable/Dev)
        // 2. /usr/share/mtc/Templates (Linux Global)
        // 3. C:\ProgramData\mtc\Templates (Windows Global - Placeholder)
        
        var portablePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "templates");
        var linuxPath = "/usr/share/mtc/Templates";
        
        if (Directory.Exists(portablePath))
        {
            _templatesPath = portablePath;
        }
        else if (Directory.Exists(linuxPath))
        {
            _templatesPath = linuxPath;
        }
        else
        {
            _templatesPath = portablePath; // Fallback
        }
    }

    public IEnumerable<Template> GetTemplates()
    {
        if (!Directory.Exists(_templatesPath))
        {
            return Enumerable.Empty<Template>();
        }

        // Search recursively for manifest.json files
        var manifestFiles = Directory.GetFiles(_templatesPath, "manifest.json", SearchOption.AllDirectories);
        var templates = new List<Template>();

        foreach (var manifestPath in manifestFiles)
        {
            var dir = Path.GetDirectoryName(manifestPath);
            if (dir == null) continue;

            try
            {
                var json = File.ReadAllText(manifestPath);
                var manifest = JsonSerializer.Deserialize<TemplateManifest>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (manifest != null)
                {
                    templates.Add(new Template(manifest, dir));
                }
            }
            catch (Exception)
            {
                // Ignore malformed manifests
            }
        }

        return templates;
    }

    public Template? GetTemplate(string name)
    {
        return GetTemplates().FirstOrDefault(t => t.Manifest.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}
