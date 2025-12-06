using Scriban;
using Scriban.Runtime;

namespace MTC.Services;

public class ScribanTemplateRenderer : ITemplateRenderer
{
    public string Render(string templateContent, Dictionary<string, object> context)
    {
        var template = Template.Parse(templateContent);
        var scriptObject = new ScriptObject();
        scriptObject.Import(context);
        
        var templateContext = new TemplateContext();
        templateContext.PushGlobal(scriptObject);
        templateContext.MemberRenamer = member => member.Name;

        return template.Render(templateContext);
    }
}
