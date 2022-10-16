﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Email.Services
{
    public interface IEmailService
    {
        Task SendEmail(string email, string subject, string body);
    }
}
