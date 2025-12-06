namespace MTC.Models;

public enum Architecture
{
    Unknown,
    MvcMonolith,
    CleanArch,
    VerticalSlice
}

public class ProjectContext
{
    public string SolutionPath { get; set; } = string.Empty;
    public string SolutionDirectory => Path.GetDirectoryName(SolutionPath) ?? string.Empty;
    public Architecture Architecture { get; set; }
    public string? MainProjectPath { get; set; }
}
