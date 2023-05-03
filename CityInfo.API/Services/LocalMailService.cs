namespace CityInfo.API.Services
{
    public class LocalMailService : IMailService
    {
        private readonly string _emailTo = string.Empty;
        private readonly string _emailFrom = string.Empty;

        public LocalMailService(IConfiguration configuration)
        {
            _emailTo = configuration["mailSettings:mailToAddress"];
            _emailFrom = configuration["mailSettings:mailFromAddress"];
        }
        public void Send(string subject, string message)
        {
            // send mail - output to console window
            Console.WriteLine($"Mail from {_emailFrom} to {_emailTo}, " +
                $"with {nameof(LocalMailService)}.");
            Console.WriteLine($"Sub: {subject}");
            Console.WriteLine($"Message: {message}");

        }
    }
}
