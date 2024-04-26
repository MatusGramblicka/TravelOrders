namespace Contracts.Exceptions;

public class CityMissingException : Exception
{
    public CityMissingException(string message) : base(message)
    {
    }
}