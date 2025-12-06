using MTC.Models;

namespace MTC.Services;

public class ContextService : IContextService
{
    public bool TryGetSolutionPath(string currentPath, out string solutionPath)
    {
        var directory = new DirectoryInfo(currentPath);
        while (directory != null)
        {
            var slnFiles = directory.GetFiles("*.sln");
            if (slnFiles.Length > 0)
            {
                solutionPath = slnFiles[0].FullName;
                return true;
            }
            directory = directory.Parent;
        }

        solutionPath = string.Empty;
        return false;
    }

    public ProjectContext Analyze(string currentPath)
    {
        var context = new ProjectContext();

        if (TryGetSolutionPath(currentPath, out var solutionPath))
        {
            context.SolutionPath = solutionPath;
            context.Architecture = DetectArchitecture(context.SolutionDirectory);
        }
        else
        {
            context.Architecture = Architecture.Unknown;
        }

        return context;
    }

    private Architecture DetectArchitecture(string solutionDirectory)
    {
        // Check for Clean Architecture
        // Look for folders or projects ending in .Domain, .Application, .Infrastructure, .API
        var directories = Directory.GetDirectories(solutionDirectory, "*", SearchOption.AllDirectories);
        
        bool hasDomain = directories.Any(d => d.EndsWith(".Domain"));
        bool hasApplication = directories.Any(d => d.EndsWith(".Application"));
        bool hasInfrastructure = directories.Any(d => d.EndsWith(".Infrastructure"));
        bool hasApi = directories.Any(d => d.EndsWith(".API"));

        if (hasDomain && hasApplication && hasInfrastructure && hasApi)
        {
            return Architecture.CleanArch;
        }

        // Check for Vertical Slice
        // Look for a "Features" folder inside any project
        bool hasFeatures = directories.Any(d => d.EndsWith("Features") && !d.Contains("node_modules")); // Basic check
        if (hasFeatures)
        {
            return Architecture.VerticalSlice;
        }

        // Check for MVC Monolith
        // Look for Controllers, Views, Models in the same directory
        foreach (var dir in directories)
        {
            var subDirs = Directory.GetDirectories(dir).Select(Path.GetFileName).ToHashSet();
            if (subDirs.Contains("Controllers") && subDirs.Contains("Views") && subDirs.Contains("Models"))
            {
                return Architecture.MvcMonolith;
            }
        }

        return Architecture.Unknown;
    }
}
