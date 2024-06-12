namespace Interface.Managers;

public interface ICsvManager
{
    Task GenerateCsv(Stream outputStream);
}