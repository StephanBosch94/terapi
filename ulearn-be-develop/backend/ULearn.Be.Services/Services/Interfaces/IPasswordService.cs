namespace ULearn.Be.Services.Services.Interfaces;

public interface IPasswordService
{
    string HashPassword(string password);
}