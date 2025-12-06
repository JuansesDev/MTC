using MTC.Models;
using Spectre.Console;

namespace MTC.Services;

public class ScaffoldingService : IScaffoldingService
{
    private readonly ITemplateRenderer _renderer;

    public ScaffoldingService(ITemplateRenderer renderer)
    {
        _renderer = renderer;
    }

    public async Task GenerateAsync(Template template, string targetDirectory, Dictionary<string, object> variables)
    {
        if (!Directory.Exists(targetDirectory))
        {
            Directory.CreateDirectory(targetDirectory);
        }

        var templateFiles = Directory.GetFiles(template.RootPath, "*", SearchOption.AllDirectories);
        Console.WriteLine($"[Debug] Found {templateFiles.Length} files in {template.RootPath}");
        
        var sourceDir = new DirectoryInfo(template.RootPath);
        await ProcessDirectoryAsync(sourceDir, targetDirectory, variables);
    }

    private async Task ProcessDirectoryAsync(DirectoryInfo sourceDir, string targetDir, Dictionary<string, object> variables)
    {
        // Copy files
        foreach (var file in sourceDir.GetFiles())
        {
            if (file.Name.Equals("manifest.json", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var targetFileName = _renderer.Render(file.Name, variables);
            var targetFilePath = Path.Combine(targetDir, targetFileName);

            // TODO: Add binary file check to avoid rendering binaries
            // For now, we assume everything is text or we try to render it. 
            // If it fails or corrupts, we'll need to add a binary check.
            
            var content = await File.ReadAllTextAsync(file.FullName);
            var renderedContent = _renderer.Render(content, variables);

            await File.WriteAllTextAsync(targetFilePath, renderedContent);
            AnsiConsole.MarkupLine($"[grey]Created file:[/] {targetFilePath}");
        }

        // Recursive copy directories
        foreach (var subDir in sourceDir.GetDirectories())
        {
            var targetSubDirName = _renderer.Render(subDir.Name, variables);
            var targetSubDirPath = Path.Combine(targetDir, targetSubDirName);

            if (!Directory.Exists(targetSubDirPath))
            {
                Directory.CreateDirectory(targetSubDirPath);
            }

            await ProcessDirectoryAsync(subDir, targetSubDirPath, variables);
        }
    }
}
