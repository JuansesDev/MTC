using System.Diagnostics;
using Xunit;

namespace MTC.IntegrationTests;

public class CliTests : IDisposable
{
    private readonly string _tempDir;
    private readonly string _mtcPath;

    public CliTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_tempDir);

        // Locate Solution Root
        var currentDir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
        while (currentDir != null && !File.Exists(Path.Combine(currentDir.FullName, "MTC.sln")))
        {
            currentDir = currentDir.Parent;
        }

        if (currentDir == null)
        {
             throw new DirectoryNotFoundException("Could not find MTC.sln in parent directories.");
        }

        var solutionDir = currentDir.FullName;
        _mtcPath = Path.Combine(solutionDir, "MTC/bin/Debug/net9.0/MTC");
        
        if (!File.Exists(_mtcPath))
        {
             throw new FileNotFoundException($"MTC binary not found at {_mtcPath}. Please build the solution first.");
        }
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDir))
        {
            Directory.Delete(_tempDir, true);
        }
    }

    [Fact]
    public void New_ConsoleApp_ShouldGenerateCompilableProject()
    {
        // 1. Generate Project
        var projectName = "TestConsoleApp";
        var exitCode = RunProcess(_mtcPath, $"new ConsoleApp -n {projectName} -o {_tempDir}");
        Assert.Equal(0, exitCode);

        // Debug: List files
        Console.WriteLine($"Listing files in {_tempDir}:");
        foreach (var file in Directory.GetFiles(_tempDir, "*", SearchOption.AllDirectories))
        {
            Console.WriteLine(file);
        }

        // 2. Verify Files Exist
        var projectDir = Path.Combine(_tempDir, projectName);
        Assert.True(Directory.Exists(projectDir), $"Project directory {projectDir} does not exist.");
        Assert.True(File.Exists(Path.Combine(projectDir, $"{projectName}.csproj")), "csproj not found.");
        Assert.True(File.Exists(Path.Combine(projectDir, "Program.cs")), "Program.cs not found.");

        // 3. Build Project
        var buildExitCode = RunProcess("dotnet", "build", projectDir);
        Assert.Equal(0, buildExitCode);
    }

    private int RunProcess(string fileName, string arguments, string? workingDirectory = null)
    {
        var psi = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            WorkingDirectory = workingDirectory ?? _tempDir,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(psi);
        process!.WaitForExit();
        
        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();
        Console.WriteLine($"Command: {fileName} {arguments}");
        Console.WriteLine($"Output: {output}");
        Console.WriteLine($"Error: {error}");

        return process.ExitCode;
    }
}
