using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;

namespace Business_logic_layer.Service
{
    public class EmailConfigurationValidator : IStartupFilter
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<EmailConfigurationValidator> _logger;

        public EmailConfigurationValidator(IOptions<EmailSettings> settings, ILogger<EmailConfigurationValidator> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            ValidateEmailSettings();
            return next;
        }

        private void ValidateEmailSettings()
        {
            _logger.LogInformation("Validating email settings on startup");

            if (string.IsNullOrEmpty(_settings.SmtpServer))
                _logger.LogWarning("SMTP server is not configured");

            if (_settings.Port <= 0)
                _logger.LogWarning("SMTP port is not configured properly");

            if (string.IsNullOrEmpty(_settings.Username))
                _logger.LogWarning("SMTP username is not configured");

            if (string.IsNullOrEmpty(_settings.Password))
                _logger.LogWarning("SMTP password is not configured");
        }

    }
}
/*
    // In Program.cs, add after configuring the EmailSender service:
   if (builder.Environment.IsDevelopment())
   {
       builder.Services.AddTransient<IStartupFilter, EmailConfigurationValidator>();
   }

   // Create this class in a new file:
   public class EmailConfigurationValidator : IStartupFilter
   {
       private readonly EmailSettings _settings;
       private readonly ILogger<EmailConfigurationValidator> _logger;

       public EmailConfigurationValidator(IOptions<EmailSettings> settings, ILogger<EmailConfigurationValidator> logger)
       {
           _settings = settings.Value;
           _logger = logger;
       }

       public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
       {
           ValidateEmailSettings();
           return next;
       }

       private void ValidateEmailSettings()
       {
           _logger.LogInformation("Validating email settings on startup");
           
           if (string.IsNullOrEmpty(_settings.SmtpServer))
               _logger.LogWarning("SMTP server is not configured");
               
           if (_settings.Port <= 0)
               _logger.LogWarning("SMTP port is not configured properly");
               
           if (string.IsNullOrEmpty(_settings.Username))
               _logger.LogWarning("SMTP username is not configured");
               
           if (string.IsNullOrEmpty(_settings.Password))
               _logger.LogWarning("SMTP password is not configured");
       }
   }
   
 */