using MTC.Models;

namespace MTC.Services;

public interface IContextService
{
    bool TryGetSolutionPath(string currentPath, out string solutionPath);
    ProjectContext Analyze(string currentPath);
}
