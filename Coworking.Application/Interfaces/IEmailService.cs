
namespace Coworking.Application.Interfaces
{
    public interface IEmailService
    {
        Task EnviaEmailAsync(string email, string subject, string body);
    }
}
