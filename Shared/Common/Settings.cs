using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Common
{
    public class Jwt{
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public int ExpiryMinutes { get; set; }
    }
    public class Settings
    {
        public SMTP SMTPSettings { get; set; }
    }
    public sealed record SMTP
    {
        public string Host { get; init; } = default!;
        public int Port { get; init; }
        public bool UseStartTls { get; init; }
        public string Username { get; init; } = default!;
        public string Password { get; init; } = default!;
        public string DisplayName { get; init; } = default!;
        public bool EnableSsl { get; init; } = true;
        public string DefaultSender { get; init; } = default!;
    }
}
