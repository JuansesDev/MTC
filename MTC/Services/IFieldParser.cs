using MTC.Models;

namespace MTC.Services;

public interface IFieldParser
{
    List<Property> Parse(string input);
}
