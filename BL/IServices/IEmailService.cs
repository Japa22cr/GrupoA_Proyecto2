using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
    public interface IEmailService
    {
        Task SendResetPasswordEmail(string email, string resetLink);
    }
}
