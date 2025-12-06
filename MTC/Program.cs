using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using MTC.Services;

namespace MTC;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        // Register services
        builder.Services.AddSingleton(AnsiConsole.Console);
        builder.Services.AddSingleton<ITemplateService, TemplateService>();
        builder.Services.AddSingleton<ITemplateRenderer, ScribanTemplateRenderer>();
        builder.Services.AddSingleton<IScaffoldingService, ScaffoldingService>();
        builder.Services.AddSingleton<IContextService, ContextService>();
        builder.Services.AddSingleton<IFieldParser, FieldParser>();
        builder.Services.AddSingleton<IUserConfigService, UserConfigService>();

        using var host = builder.Build();

        // Setup CLI
        var rootCommand = new RootCommand("MTC - Modular Template CLI");

        rootCommand.SetHandler(() =>
        {
            AnsiConsole.Write(
                new FigletText("MTC")
                    .LeftJustified()
                    .Color(Color.Blue));
            AnsiConsole.MarkupLine("[bold blue]Welcome to MTC (Modular Template CLI)[/]");
            AnsiConsole.MarkupLine("Use [green]--help[/] to see available commands.");
        });

        // 'new' command
        var newCommand = new Command("new", "Create a new project from a template");
        var templateNameArg = new Argument<string>("template-name", "The name of the template to use");
        var outputOption = new Option<string>(new[] { "--output", "-o" }, () => ".", "The output directory");
        var nameOption = new Option<string>(new[] { "--name", "-n" }, "The name of the project");

        newCommand.AddArgument(templateNameArg);
        newCommand.AddOption(outputOption);
        newCommand.AddOption(nameOption);

        newCommand.SetHandler(async (string templateName, string output, string name) =>
        {
            var templateService = host.Services.GetRequiredService<ITemplateService>();
            var scaffoldingService = host.Services.GetRequiredService<IScaffoldingService>();

            var template = templateService.GetTemplate(templateName);
            if (template == null)
            {
                AnsiConsole.MarkupLine($"[red]Error:[/] Template '{templateName}' not found.");
                return;
            }

            // Default output to current directory if not specified
            if (string.IsNullOrEmpty(output))
            {
                output = Directory.GetCurrentDirectory();
            }

            // Default name to output directory name if not specified
            if (string.IsNullOrEmpty(name))
            {
                name = new DirectoryInfo(output).Name;
            }

            var variables = new Dictionary<string, object>
            {
                { "Name", name }
                // TODO: Collect other variables defined in manifest
            };

            AnsiConsole.MarkupLine($"[bold green]Creating project '{name}' from template '{templateName}'...[/]");
            AnsiConsole.MarkupLine($"[yellow]Debug:[/] Template Directory: {template.RootPath}");
            
            try 
            {
                await scaffoldingService.GenerateAsync(template, output, variables);
                AnsiConsole.MarkupLine("[bold green]Done![/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
                Environment.ExitCode = 1;
            }

        }, templateNameArg, outputOption, nameOption);

        rootCommand.AddCommand(newCommand);

        // 'add' command group
        var addCommand = new Command("add", "Add a new item to the project");
        
        // 'add feature' command
        var addFeatureCommand = new Command("feature", "Add a new feature slice");
        var featureNameArg = new Argument<string>("name", "The name of the feature");
        var fieldsOption = new Option<string>("--fields", "The fields of the entity (e.g. 'Name:string Price:decimal')");

        addFeatureCommand.AddArgument(featureNameArg);
        addFeatureCommand.AddOption(fieldsOption);

        addFeatureCommand.SetHandler(async (string name, string fields) =>
        {
            var contextService = host.Services.GetRequiredService<IContextService>();
            var templateService = host.Services.GetRequiredService<ITemplateService>();
            var scaffoldingService = host.Services.GetRequiredService<IScaffoldingService>();
            var fieldParser = host.Services.GetRequiredService<IFieldParser>();

            var currentPath = Directory.GetCurrentDirectory();
            var context = contextService.Analyze(currentPath);

            if (context.Architecture == MTC.Models.Architecture.Unknown)
            {
                AnsiConsole.MarkupLine("[red]Error:[/] Unknown project architecture.");
                return;
            }

            if (string.IsNullOrEmpty(context.SolutionPath))
            {
                AnsiConsole.MarkupLine("[red]Error:[/] Could not find solution file.");
                return;
            }

            var properties = fieldParser.Parse(fields);
            // Use Solution Name as base ProjectName
            var projectName = Path.GetFileNameWithoutExtension(context.SolutionPath);
            if (string.IsNullOrEmpty(projectName))
            {
                projectName = "MyProject";
            }

            var variables = new Dictionary<string, object>
            {
                { "Name", name },
                { "Properties", properties },
                { "ProjectName", projectName }
            };

            if (context.Architecture == MTC.Models.Architecture.VerticalSlice)
            {
                // Vertical Slice Logic
                string featuresPath = string.Empty;
                var featuresDirs = Directory.GetDirectories(context.SolutionDirectory, "Features", SearchOption.AllDirectories);
                if (featuresDirs.Length == 0)
                {
                     AnsiConsole.MarkupLine("[red]Error:[/] Could not find 'Features' directory.");
                     return;
                }
                featuresPath = featuresDirs[0]; 
                var targetDir = Path.Combine(featuresPath, name);
                
                var template = templateService.GetTemplate("VerticalSliceFeature");
                if (template == null) { AnsiConsole.MarkupLine("[red]Error:[/] Template 'VerticalSliceFeature' not found."); return; }

                AnsiConsole.MarkupLine($"[bold green]Creating feature '{name}' in '{featuresPath}'...[/]");
                try { await scaffoldingService.GenerateAsync(template, featuresPath, variables); AnsiConsole.MarkupLine("[bold green]Done![/]"); }
                catch (Exception ex) { AnsiConsole.WriteException(ex); }
            }
            else if (context.Architecture == MTC.Models.Architecture.MvcMonolith)
            {
                // MVC Logic
                var template = templateService.GetTemplate("MvcFeature");
                if (template == null) { AnsiConsole.MarkupLine("[red]Error:[/] Template 'MvcFeature' not found."); return; }

                // Target is the project root containing Controllers/Models/Views
                // We assume the current directory is the project root or we can find it
                // For simplicity, let's use context.SolutionDirectory and find the project folder
                var projectDir = Directory.GetDirectories(context.SolutionDirectory).FirstOrDefault(d => Directory.Exists(Path.Combine(d, "Controllers")));
                if (projectDir == null) { AnsiConsole.MarkupLine("[red]Error:[/] Could not find MVC project root."); return; }

                AnsiConsole.MarkupLine($"[bold green]Creating MVC resource '{name}' in '{projectDir}'...[/]");
                try { await scaffoldingService.GenerateAsync(template, projectDir, variables); AnsiConsole.MarkupLine("[bold green]Done![/]"); }
                catch (Exception ex) { AnsiConsole.WriteException(ex); }
            }
            else if (context.Architecture == MTC.Models.Architecture.CleanArch)
            {
                // Clean Arch Logic
                var template = templateService.GetTemplate("CleanArchFeature");
                if (template == null) { AnsiConsole.MarkupLine("[red]Error:[/] Template 'CleanArchFeature' not found."); return; }

                // We need to inject into multiple projects (Domain, Application, API)
                // ScaffoldingService generates relative to output dir.
                // Our template structure mirrors the folders: Domain/Entities, Application/Features, API/Controllers
                // So we can target the Solution Directory directly?
                // No, the template has folders like "Domain/Entities".
                // If we output to SolutionDirectory, it will try to create SolutionDirectory/Domain/Entities
                // But the actual path might be SolutionDirectory/ProjectName.Domain/Entities
                // This is tricky because project names vary.
                
                // Workaround: We need to find the actual paths for Domain, Application, API projects.
                var domainDir = Directory.GetDirectories(context.SolutionDirectory, "*.Domain", SearchOption.AllDirectories).FirstOrDefault();
                var appDir = Directory.GetDirectories(context.SolutionDirectory, "*.Application", SearchOption.AllDirectories).FirstOrDefault();
                var apiDir = Directory.GetDirectories(context.SolutionDirectory, "*.API", SearchOption.AllDirectories).FirstOrDefault();

                if (domainDir == null || appDir == null || apiDir == null)
                {
                    AnsiConsole.MarkupLine("[red]Error:[/] Could not find one or more Clean Architecture layers.");
                    return;
                }

                // We will generate piece by piece or adjust the template?
                // Adjusting the template to match exact project names is hard.
                // Better approach: The template has "Domain", "Application", "API" folders.
                // We can generate into a temp folder and then move files? Or update ScaffoldingService?
                
                // Let's try to map the template folders to the actual project folders.
                // Template: Domain/Entities/{{Name}}.cs -> Actual: MyProject.Domain/Entities/{{Name}}.cs
                
                // We can run the generator 3 times, pointing to each project, using partial templates?
                // Or we can assume standard naming and generate into SolutionDir, but that risks mismatch if project folders are named "MyProject.Domain".
                
                // Let's assume standard CleanArch template structure:
                // SolutionDir/
                //   MyProject.Domain/
                //   MyProject.Application/
                //   MyProject.API/

                // If we generate "Domain/Entities/..." into SolutionDir, we get "SolutionDir/Domain/Entities/..."
                // We want "SolutionDir/MyProject.Domain/Entities/..."
                
                // Hack: We can pass the project folder names as variables to the template?
                // But the template folder structure is static.
                
                // Alternative: Update ScaffoldingService to handle mapping? Too complex.
                
                // Simple solution for now:
                // Generate into a temp folder, then move files to correct locations.
                
                var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                Directory.CreateDirectory(tempDir);

                try 
                {
                    await scaffoldingService.GenerateAsync(template, tempDir, variables);
                    
                    // Move Domain
                    var sourceDomain = Path.Combine(tempDir, "Domain");
                    CopyFilesRecursively(sourceDomain, domainDir);

                    // Move Application
                    var sourceApp = Path.Combine(tempDir, "Application");
                    CopyFilesRecursively(sourceApp, appDir);

                    // Move API
                    var sourceApi = Path.Combine(tempDir, "API");
                    CopyFilesRecursively(sourceApi, apiDir);

                    AnsiConsole.MarkupLine("[bold green]Done![/]");
                }
                catch (Exception ex) { AnsiConsole.WriteException(ex); }
                finally { Directory.Delete(tempDir, true); }
            }

        }, featureNameArg, fieldsOption);

        addCommand.AddCommand(addFeatureCommand);

        // Add Value Object Command
        var addValueObjectCommand = new Command("value-object", "Add a new Value Object");
        addValueObjectCommand.AddArgument(featureNameArg);
        addValueObjectCommand.AddOption(fieldsOption);
        addValueObjectCommand.SetHandler(async (name, fields) =>
        {
            var contextService = host.Services.GetRequiredService<IContextService>();
            var templateService = host.Services.GetRequiredService<ITemplateService>();
            var scaffoldingService = host.Services.GetRequiredService<IScaffoldingService>();
            var fieldParser = host.Services.GetRequiredService<IFieldParser>();

            var currentPath = Directory.GetCurrentDirectory();
            var context = contextService.Analyze(currentPath);

            if (context.Architecture == MTC.Models.Architecture.Unknown)
            {
                AnsiConsole.MarkupLine("[red]Error:[/] Unknown project architecture.");
                return;
            }

            var properties = fieldParser.Parse(fields);
            var projectName = Path.GetFileNameWithoutExtension(context.SolutionPath) ?? "MyProject";
            string targetNamespace = $"{projectName}.ValueObjects";
            string targetDir = string.Empty;

            if (context.Architecture == MTC.Models.Architecture.CleanArch)
            {
                var domainDir = Directory.GetDirectories(context.SolutionDirectory, "*.Domain", SearchOption.AllDirectories).FirstOrDefault();
                if (domainDir != null)
                {
                    targetDir = Path.Combine(domainDir, "ValueObjects");
                    targetNamespace = $"{new DirectoryInfo(domainDir).Name}.ValueObjects";
                }
            }
            else if (context.Architecture == MTC.Models.Architecture.MvcMonolith)
            {
                 var projectDir = Directory.GetDirectories(context.SolutionDirectory).FirstOrDefault(d => Directory.Exists(Path.Combine(d, "Controllers")));
                 if (projectDir != null)
                 {
                     targetDir = Path.Combine(projectDir, "Models", "ValueObjects");
                     targetNamespace = $"{new DirectoryInfo(projectDir).Name}.Models.ValueObjects";
                 }
            }
            else // Vertical Slice or other
            {
                // Default to current directory or Features/Shared
                targetDir = Path.Combine(currentPath, "ValueObjects");
            }

            if (string.IsNullOrEmpty(targetDir))
            {
                 AnsiConsole.MarkupLine("[red]Error:[/] Could not determine target directory.");
                 return;
            }

            var variables = new Dictionary<string, object>
            {
                { "Name", name },
                { "Properties", properties },
                { "Namespace", targetNamespace }
            };

            var template = templateService.GetTemplate("ValueObject");
            if (template == null) { AnsiConsole.MarkupLine("[red]Error:[/] Template 'ValueObject' not found."); return; }

            AnsiConsole.MarkupLine($"[bold green]Creating Value Object '{name}' in '{targetDir}'...[/]");
            try { await scaffoldingService.GenerateAsync(template, targetDir, variables); AnsiConsole.MarkupLine("[bold green]Done![/]"); }
            catch (Exception ex) { AnsiConsole.WriteException(ex); }

        }, featureNameArg, fieldsOption);
        addCommand.AddCommand(addValueObjectCommand);

        // Add DTO Command
        var addDtoCommand = new Command("dto", "Add a new DTO");
        addDtoCommand.AddArgument(featureNameArg);
        addDtoCommand.AddOption(fieldsOption);
        addDtoCommand.SetHandler(async (name, fields) =>
        {
            var contextService = host.Services.GetRequiredService<IContextService>();
            var templateService = host.Services.GetRequiredService<ITemplateService>();
            var scaffoldingService = host.Services.GetRequiredService<IScaffoldingService>();
            var fieldParser = host.Services.GetRequiredService<IFieldParser>();

            var currentPath = Directory.GetCurrentDirectory();
            var context = contextService.Analyze(currentPath);

            if (context.Architecture == MTC.Models.Architecture.Unknown)
            {
                AnsiConsole.MarkupLine("[red]Error:[/] Unknown project architecture.");
                return;
            }

            var properties = fieldParser.Parse(fields);
            var projectName = Path.GetFileNameWithoutExtension(context.SolutionPath) ?? "MyProject";
            string targetNamespace = $"{projectName}.DTOs";
            string targetDir = string.Empty;

            if (context.Architecture == MTC.Models.Architecture.CleanArch)
            {
                var appDir = Directory.GetDirectories(context.SolutionDirectory, "*.Application", SearchOption.AllDirectories).FirstOrDefault();
                if (appDir != null)
                {
                    targetDir = Path.Combine(appDir, "DTOs");
                    targetNamespace = $"{new DirectoryInfo(appDir).Name}.DTOs";
                }
            }
            else if (context.Architecture == MTC.Models.Architecture.MvcMonolith)
            {
                 var projectDir = Directory.GetDirectories(context.SolutionDirectory).FirstOrDefault(d => Directory.Exists(Path.Combine(d, "Controllers")));
                 if (projectDir != null)
                 {
                     targetDir = Path.Combine(projectDir, "Models", "DTOs");
                     targetNamespace = $"{new DirectoryInfo(projectDir).Name}.Models.DTOs";
                 }
            }
            else // Vertical Slice or other
            {
                targetDir = Path.Combine(currentPath, "DTOs");
            }

            if (string.IsNullOrEmpty(targetDir))
            {
                 AnsiConsole.MarkupLine("[red]Error:[/] Could not determine target directory.");
                 return;
            }

            var variables = new Dictionary<string, object>
            {
                { "Name", name },
                { "Properties", properties },
                { "Namespace", targetNamespace }
            };

            var template = templateService.GetTemplate("Dto");
            if (template == null) { AnsiConsole.MarkupLine("[red]Error:[/] Template 'Dto' not found."); return; }

            AnsiConsole.MarkupLine($"[bold green]Creating DTO '{name}' in '{targetDir}'...[/]");
            try { await scaffoldingService.GenerateAsync(template, targetDir, variables); AnsiConsole.MarkupLine("[bold green]Done![/]"); }
            catch (Exception ex) { AnsiConsole.WriteException(ex); }

        }, featureNameArg, fieldsOption);
        addCommand.AddCommand(addDtoCommand);
        rootCommand.AddCommand(addCommand);

        // Config Command
        var configCommand = new Command("config", "Manage user configuration");

        var configSetCommand = new Command("set", "Set a configuration value");
        var keyArg = new Argument<string>("key", "Configuration key");
        var valueArg = new Argument<string>("value", "Configuration value");
        configSetCommand.AddArgument(keyArg);
        configSetCommand.AddArgument(valueArg);
        configSetCommand.SetHandler((key, value) =>
        {
            var configService = host.Services.GetRequiredService<IUserConfigService>();
            configService.Set(key, value);
            AnsiConsole.MarkupLine($"[green]Configuration set:[/] {key} = {value}");
        }, keyArg, valueArg);

        var configGetCommand = new Command("get", "Get a configuration value");
        configGetCommand.AddArgument(keyArg);
        configGetCommand.SetHandler((key) =>
        {
            var configService = host.Services.GetRequiredService<IUserConfigService>();
            var value = configService.Get(key);
            if (value != null)
            {
                AnsiConsole.MarkupLine($"[green]{key}:[/] {value}");
            }
            else
            {
                AnsiConsole.MarkupLine($"[yellow]Configuration key '{key}' not set.[/]");
            }
        }, keyArg);

        var configListCommand = new Command("list", "List all configuration values");
        configListCommand.SetHandler(() =>
        {
            var configService = host.Services.GetRequiredService<IUserConfigService>();
            var all = configService.GetAll();
            if (all.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No configuration set.[/]");
                return;
            }

            var table = new Table();
            table.AddColumn("Key");
            table.AddColumn("Value");

            foreach (var kvp in all)
            {
                table.AddRow(kvp.Key, kvp.Value);
            }

            AnsiConsole.Write(table);
        });

        configCommand.AddCommand(configSetCommand);
        configCommand.AddCommand(configGetCommand);
        configCommand.AddCommand(configListCommand);
        rootCommand.AddCommand(configCommand);

        // List command
        var listCommand = new Command("list", "List available templates");
        listCommand.SetHandler(() =>
        {
            var templateService = host.Services.GetRequiredService<ITemplateService>();
            var templates = templateService.GetTemplates();
            
            var table = new Table();
            table.AddColumn("Name");
            table.AddColumn("Description");
            table.AddColumn("Author");
            table.AddColumn("Version");

            foreach (var template in templates)
            {
                table.AddRow(
                    template.Manifest.Name, 
                    template.Manifest.Description,
                    template.Manifest.Author,
                    template.Manifest.Version
                );
            }

            AnsiConsole.Write(table);
        });
        // Debug Context Command
        var debugContextCommand = new Command("debug-context", "Debug the current context detection");
        debugContextCommand.SetHandler(() =>
        {
            var contextService = host.Services.GetRequiredService<IContextService>();
            var currentPath = Directory.GetCurrentDirectory();
            var context = contextService.Analyze(currentPath);

            AnsiConsole.MarkupLine($"[bold]Current Path:[/] {currentPath}");
            AnsiConsole.MarkupLine($"[bold]Solution Path:[/] {(string.IsNullOrEmpty(context.SolutionPath) ? "[red]Not Found[/]" : context.SolutionPath)}");
            AnsiConsole.MarkupLine($"[bold]Architecture:[/] {context.Architecture}");
        });
        rootCommand.AddCommand(debugContextCommand);

        rootCommand.AddCommand(listCommand);

        var parser = new CommandLineBuilder(rootCommand)
            .UseDefaults()
            .Build();

        await parser.InvokeAsync(args);
    }

    private static void CopyFilesRecursively(string sourcePath, string targetPath)
    {
        // Now Create all of the directories
        foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
        {
            Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
        }

        // Copy all the files & Replaces any files with the same name
        foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
        {
            File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
        }
    }
}
