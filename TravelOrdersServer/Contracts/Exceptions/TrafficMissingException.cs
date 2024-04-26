namespace Contracts.Exceptions;

public class TrafficMissingException : Exception
{
    public TrafficMissingException(string message) : base(message)
    {
    }
}