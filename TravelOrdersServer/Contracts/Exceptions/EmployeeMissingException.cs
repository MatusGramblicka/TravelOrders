namespace Contracts.Exceptions;

public class EmployeeMissingException : Exception
{
    public EmployeeMissingException(string message) : base(message)
    {
    }
}