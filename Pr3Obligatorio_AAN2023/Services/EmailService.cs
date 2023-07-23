using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Pr3Obligatorio_AAN2023.Interfaces;
using System;
using System.Threading.Tasks;

namespace Pr3Obligatorio_AAN2023.Servicios
{
    public class EmailService : IEmailService
    {
        public async Task SendConfirmationEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Cinecito", "aguacatehamburguesas@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));

            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain")
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync("aguacatehamburguesas@gmail.com", "HolaSoyAlexis2768");
                    await client.SendAsync(emailMessage);
                }
                catch (Exception ex)
                {
                    // Manejar cualquier excepción que pueda ocurrir durante el envío del correo electrónico
                    // Por ejemplo, puedes registrar el error o enviar una notificación al administrador del sistema.
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }
            }
        }
    }
}
