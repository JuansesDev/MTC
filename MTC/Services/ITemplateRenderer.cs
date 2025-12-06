namespace MTC.Services;

public interface ITemplateRenderer
{
    string Render(string templateContent, Dictionary<string, object> context);
}
