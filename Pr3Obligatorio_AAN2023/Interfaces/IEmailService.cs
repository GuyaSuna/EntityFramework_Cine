using System.Threading.Tasks;

namespace Pr3Obligatorio_AAN2023.Interfaces
{
    public interface IEmailService
    {
        Task SendConfirmationEmailAsync(string email, string subject, string message);
    }
}