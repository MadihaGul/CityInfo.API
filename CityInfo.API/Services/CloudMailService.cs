namespace CityInfo.API.Services
{
    public class CloudMailService : IMailService
    {
        private readonly string _emailTo = string.Empty;
        private readonly string _emailFrom = string.Empty;
        public CloudMailService(IConfiguration configuration)
        {
            _emailTo = configuration["mailSettings:mailToAddress"];
            _emailFrom = configuration["mailSettings:mailFromAddress"];
        }
        public void Send(string subject, string message)
        {
            // send mail - output to console window
            Console.WriteLine($"Mail from {_emailFrom} to {_emailTo}, " +
                $"with {nameof(CloudMailService)}.");
            Console.WriteLine($"Sub: {subject}");
            Console.WriteLine($"Message: {message}");

        }
    }
}
