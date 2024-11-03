using BL.IServices;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BL.Services
{
    public class EmailService: IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration) 
        {
            _configuration = configuration;
        }
        public async Task SendResetPasswordEmail(string email, string resetUrl) 
        {
            var apikey = _configuration["SendGrid:ApiKey"];
            var fromEmail = _configuration["SendGrid:FromEmail"];
            var client = new SendGridClient(apikey);
            var from = new EmailAddress("ugaldelenin@outlook.es", "Transito Inteligente");
            var subject = "Recuperacion de Contrasena";
            var to = new EmailAddress(email);
            var plainTextContent = $"Haga click aqui para reestablecer su contrasena:<br><br>  {resetUrl}";
            var htmlContent = $"<strong>Haga click aqui para reestablecer su contrasena:</strong> <a href='{resetUrl}'>Reestablecer Contrasena</a> ";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            // Log response details
            var statusCode = response.StatusCode;
            var body = await response.Body.ReadAsStringAsync();
            var headers = response.Headers;

            //Codigo para debugging. Eliminar este bloque una vez que todo funcione correctamente.
            // Log the information or print it to the console
            Console.WriteLine($"Status Code: {statusCode}");
            Console.WriteLine($"Body: {body}");
            foreach (var header in headers)
            {
                Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
            }
        }
    }
}
