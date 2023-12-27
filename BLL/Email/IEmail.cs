using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Email
{
    public interface IEmail
    {
        bool SendEmail(string Subject, string Body, string recipientEmail);
    }
}
