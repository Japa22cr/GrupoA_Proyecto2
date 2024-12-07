using DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendResetPasswordEmail(string email, string resetLink);
        Task SendFineConfirmationEmailOfficer(string email, Fine fine);
        Task SendFineAlertEmailUser(string email, Fine fine);
        Task SendPaymentConfirmationEmail(string email, Fine fine, Payment payment);
        Task SendDisputeConfirmationEmail(string email, Dispute dispute, Fine fine);
        Task SendDisputeResolutionOfficer(string email, Dispute dispute);
        Task SendDisputeResolutionUser(string email, Dispute dispute);
    }
}
