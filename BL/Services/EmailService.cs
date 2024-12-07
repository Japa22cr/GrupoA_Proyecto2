using BL.IServices;
using DataAccess.EF.Models;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BL.Services
{
    public class EmailService: IEmailService
    {
        private readonly IConfiguration _configuration;
        private object _sendGridClient;

        public EmailService(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {

            var apikey = _configuration["SendGrid:ApiKey2"];
            var client = new SendGridClient(apikey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("transitointeligentecr@gmail.com", "Transito Inteligente"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = $"<strong>{message}</strong>"
            };
            msg.AddTo(new EmailAddress(email));

            var response = await client.SendEmailAsync(msg);
        }

        public async Task SendResetPasswordEmail(string email, string resetUrl) 
        {
            var apikey = _configuration["SendGrid:ApiKey"];
            var fromEmail = _configuration["SendGrid:FromEmail"];
            var client = new SendGridClient(apikey);
            var from = new EmailAddress("ugaldelenin@outlook.es", "Transito Inteligente");
            var subject = "Recuperacion de Contraseña";
            var to = new EmailAddress(email);
            var plainTextContent = $"Haga click aqui para reestablecer su contraseña:<br><br>  {resetUrl}";
            var htmlContent = $"<strong>Haga click aqui para reestablecer su contraseña:</strong> <a href='{resetUrl}'>Reestablecer Contrasena</a> ";
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

        public async Task SendFineConfirmationEmailOfficer(string email, Fine fine)
        {
            var apikey = _configuration["SendGrid:ApiKey"];
            var fromEmail = _configuration["SendGrid:FromEmail"];
            var client = new SendGridClient(apikey);
            var from = new EmailAddress("ugaldelenin@outlook.es", "Transito Inteligente");
            var subject = "Confirmación de Multa";
            var to = new EmailAddress(email);
            var plainTextContent = $"";
            var htmlContent = $@"
                <p>Querido Oficial,</p>
                <p>la multa efectuado el {fine.IssuedDate} fue procesada exitosamente.</p>
                <p><strong>Detalles de la Multa:</strong></p>
                <ul>
                    <li>Multa ID: {fine.Id}</li>
                    <li>Inspector: ${fine.Amount}</li>
                    <li>No Zona: {fine.Place}</li>
                    <li>Categoria: {fine.Category}</li>
                    <li>Ley de transito: {fine.Article}</li>
                    <li>Conducta: {fine.Conduct}</li>
                    <li>Conducta: {fine.Description}</li>
                    <li>monto: ${fine.Amount}</li>
                </ul>
                <p>Buen dia,<br>Transito Inteligente</p>";
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

        public async Task SendFineAlertEmailUser(string email, Fine fine)
        {
            var apikey = _configuration["SendGrid:ApiKey"];
            var fromEmail = _configuration["SendGrid:FromEmail"];
            var client = new SendGridClient(apikey);
            var from = new EmailAddress("ugaldelenin@outlook.es", "Transito Inteligente");
            var subject = "Alerta de Multa";
            var to = new EmailAddress(email);
            var plainTextContent = $"";
            var htmlContent = $@"
                <p>Querido Usuario,</p>
                <p>la multa efectuado el {fine.IssuedDate} fue procesada exitosamente.</p>
                <p><strong>Detalles de la Multa:</strong></p>
                <ul>
                    <li>Multa ID: {fine.Id}</li>
                    <li>Inspector: ${fine.Amount}</li>
                    <li>No Zona: {fine.Place}</li>
                    <li>Categoria: {fine.Category}</li>
                    <li>Ley de transito: {fine.Article}</li>
                    <li>Conducta: {fine.Conduct}</li>
                    <li>Conducta: {fine.Description}</li>
                    <li>monto: ${fine.Amount}</li>
                </ul>
                <p>Aconsejamos resolver la multa lo mas pronto posible.</p>
                <p>Buen dia,<br>Transito Inteligente</p>";
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

        public async Task SendPaymentConfirmationEmail(string email, Fine fine, Payment payment)
        {
            var apikey = _configuration["SendGrid:ApiKey"];
            var fromEmail = _configuration["SendGrid:FromEmail"];
            var client = new SendGridClient(apikey);
            var from = new EmailAddress("ugaldelenin@outlook.es", "Transito Inteligente");
            var subject = "Confirmación de Pago";
            var to = new EmailAddress(email);
            var plainTextContent = $"";
            var htmlContent = $@"
                <p>Querido Usuario,</p>
                <p>Su pago por la multa efectuado {fine.IssuedDate} fue procesado exitosamente.</p>
                <p><strong>Detalles de Pago:</strong></p>
                <ul>
                    <li>Multa ID: {fine.Id}</li>
                    <li>Monto: ${payment.Amount}</li>
                    <li>Método de Pago: {payment.PaymentMethod}</li>
                    <li>Fecha de Pago: {payment.PaymentDate}</li>
                </ul>
                <p>Muchas Gracias por resolver la multa.</p>
                <p>Buen dia,<br>Transito Inteligente</p>";
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

        public async Task SendDisputeConfirmationEmail(string email, Dispute dispute,Fine fine)
        {
            var apikey = _configuration["SendGrid:ApiKey"];
            var fromEmail = _configuration["SendGrid:FromEmail"];
            var client = new SendGridClient(apikey);
            var from = new EmailAddress("ugaldelenin@outlook.es", "Transito Inteligente");
            var subject = "Confirmación de la disputa";
            var to = new EmailAddress(email);
            var plainTextContent = $"";
            var htmlContent = $@"
                <p>Querido Usuario,</p>
                <p>la multa fue disputada exitosamente.</p>
                <p><strong>Detalles de la Multa:</strong></p>
                <ul>
                    <li>Multa ID: {fine.Id}</li>
                    <li>Inspector: ${fine.Amount}</li>
                    <li>No Zona: {fine.Place}</li>
                    <li>Categoria: {fine.Category}</li>
                    <li>Ley de transito: {fine.Article}</li>
                    <li>Conducta: {fine.Conduct}</li>
                    <li>Conducta: {fine.Description}</li>
                    <li>monto: ${fine.Amount}</li>
                </ul>

                <p><strong>Detalles de la disputa:</strong></p>
                <ul>
                    <li>Disputa ID: {dispute.Id}</li>
                    <li>Razón: ${dispute.Reason}</li>
                    <li>Fecha de creación: {dispute.CreatedDate}</li>
                </ul>

                <p>Esperamos darle una respuesta lo mas pronto posible.</p>
                <p>Buen dia,<br>Transito Inteligente</p>";
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

        public async Task SendDisputeResolutionOfficer(string email, Dispute dispute)
        {
            var apikey = _configuration["SendGrid:ApiKey"];
            var fromEmail = _configuration["SendGrid:FromEmail"];
            var client = new SendGridClient(apikey);
            var from = new EmailAddress("ugaldelenin@outlook.es", "Transito Inteligente");
            var subject = "Resolución de la disputa";
            var to = new EmailAddress(email);
            var plainTextContent = $"";
            var htmlContent = $@"
                <p>Querido Oficial,</p>
                <p>la disputa efectuado el {dispute.CreatedDate} ya fue resuelta.</p>
                <p><strong>Detalles de la resolucion de la disputa:</strong></p>
                <ul>
                    <li>Disputa ID: {dispute.Id}</li>
                    <li>Razón proporcionada: ${dispute.Reason}</li>
                    <li>Juez: ${dispute.Judge.UserName}</li>
                    <li>Resolución: ${dispute.Resolution}</li>
                    <li>Fecha de resolución: ${dispute.ResolutionDate}</li>
                </ul>
                <p>Agradecemos su paciencia en esta situación.</p>
                <p>Buen dia,<br>Transito Inteligente</p>";
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

        public async Task SendDisputeResolutionUser(string email, Dispute dispute)
        {
            var apikey = _configuration["SendGrid:ApiKey"];
            var fromEmail = _configuration["SendGrid:FromEmail"];
            var client = new SendGridClient(apikey);
            var from = new EmailAddress("ugaldelenin@outlook.es", "Transito Inteligente");
            var subject = "Resolución de la disputa";
            var to = new EmailAddress(email);
            var plainTextContent = $"";
            var htmlContent = $@"
                <p>Querido Usuario,</p>
                <p>la disputa efectuado el {dispute.CreatedDate} ya fue resuelta.</p>
                <p><strong>Detalles de la resolucion de la disputa:</strong></p>
                <ul>
                    <li>Disputa ID: {dispute.Id}</li>
                    <li>Razón proporcionada: ${dispute.Reason}</li>
                    <li>Juez: ${dispute.Judge.UserName}</li>
                    <li>Resolución: ${dispute.Resolution}</li>
                    <li>Fecha de resolución: ${dispute.ResolutionDate}</li>
                </ul>
                <p>Agradecemos su paciencia en esta situación.</p>
                <p>Buen dia,<br>Transito Inteligente</p>";
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
