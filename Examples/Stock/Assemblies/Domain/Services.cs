using Stock.Domain.Contracts;

namespace Stock.Domain
{
    public static class Services
    {
        public static ILogService Logger { get; set; } = null;
        public static IEmailService Mailer { get; set; } = null;
    }
}
