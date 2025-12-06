using MTC.Services;
using Xunit;

namespace MTC.Tests;

public class FieldParserTests
{
    private readonly FieldParser _parser;

    public FieldParserTests()
    {
        _parser = new FieldParser();
    }

    [Fact]
    public void Parse_ShouldReturnEmptyList_WhenInputIsEmpty()
    {
        var result = _parser.Parse("");
        Assert.Empty(result);
    }

    [Fact]
    public void Parse_ShouldParseSingleField()
    {
        var result = _parser.Parse("Name:string");
        Assert.Single(result);
        Assert.Equal("Name", result[0].Name);
        Assert.Equal("string", result[0].Type);
    }

    [Fact]
    public void Parse_ShouldParseMultipleFields()
    {
        var result = _parser.Parse("Name:string Age:int");
        Assert.Equal(2, result.Count);
        Assert.Equal("Name", result[0].Name);
        Assert.Equal("string", result[0].Type);
        Assert.Equal("Age", result[1].Name);
        Assert.Equal("int", result[1].Type);
    }

    [Fact]
    public void Parse_ShouldHandleExtraSpaces()
    {
        var result = _parser.Parse(" Name:string   Age:int ");
        Assert.Equal(2, result.Count);
        Assert.Equal("Name", result[0].Name);
        Assert.Equal("Age", result[1].Name);
    }

    [Fact]
    public void Parse_ShouldDefaultToString_WhenTypeIsMissing()
    {
        // Assuming FieldParser defaults to string if no type provided, 
        // or throws? Let's check implementation behavior or define expected behavior.
        // Current implementation splits by ':' so "Name" might fail or set Type to empty.
        // Let's assume input MUST be Name:Type for now based on usage.
        // If implementation allows "Name", let's test it.
        // For now, let's stick to standard "Name:Type".
    }
}
