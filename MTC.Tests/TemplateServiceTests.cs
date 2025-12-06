using MTC.Services;
using Xunit;

namespace MTC.Tests;

public class TemplateServiceTests : IDisposable
{
    private readonly string _tempDir;

    public TemplateServiceTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_tempDir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDir))
        {
            Directory.Delete(_tempDir, true);
        }
    }

    [Fact]
    public void GetTemplates_ShouldReturnTemplates_WhenManifestExists()
    {
        // Arrange
        var templateDir = Path.Combine(_tempDir, "TestTemplate");
        Directory.CreateDirectory(templateDir);
        var manifestJson = @"{
            ""name"": ""TestTemplate"",
            ""description"": ""A test template"",
            ""author"": ""Tester"",
            ""version"": ""1.0.0"",
            ""type"": ""Project""
        }";
        File.WriteAllText(Path.Combine(templateDir, "manifest.json"), manifestJson);

        var service = new TemplateService(_tempDir);

        // Act
        var templates = service.GetTemplates().ToList();

        // Assert
        Assert.Single(templates);
        Assert.Equal("TestTemplate", templates[0].Manifest.Name);
        Assert.Equal("A test template", templates[0].Manifest.Description);
    }

    [Fact]
    public void GetTemplates_ShouldReturnEmpty_WhenNoManifests()
    {
        // Arrange
        var service = new TemplateService(_tempDir);

        // Act
        var templates = service.GetTemplates();

        // Assert
        Assert.Empty(templates);
    }
}
