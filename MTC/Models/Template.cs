namespace MTC.Models;

public class Template
{
    public TemplateManifest Manifest { get; set; }
    public string RootPath { get; set; }

    public Template(TemplateManifest manifest, string rootPath)
    {
        Manifest = manifest;
        RootPath = rootPath;
    }
}
