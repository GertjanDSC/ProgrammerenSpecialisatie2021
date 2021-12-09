namespace Stock.Domain.Contracts
{
    public interface IEmailService
    {
        string Host { get; set; }
        int Port { get; set; }
        string User { get; set; }
        string Password { get; set; }

        void Send(string from, string[] to, string subject, string html);
    }
}
