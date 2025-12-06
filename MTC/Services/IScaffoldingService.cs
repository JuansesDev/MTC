using MTC.Models;

namespace MTC.Services;

public interface IScaffoldingService
{
    Task GenerateAsync(Template template, string targetDirectory, Dictionary<string, object> variables);
}
