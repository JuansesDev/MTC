using MTC.Models;

namespace MTC.Services;

public interface ITemplateService
{
    IEnumerable<Template> GetTemplates();
    Template? GetTemplate(string name);
}
