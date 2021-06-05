using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestForGraduates.Interfaces
{
    public interface IMessageEmailService
    {
        Task SendEmailMessage(string email, string subject, string message);
    }
}
