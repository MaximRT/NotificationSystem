namespace NotificationSystem.Services
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; } = "";
        public int Port { get; set; } = 465;
        public bool UseSsl { get; set; } = true;
        public string FromEmail { get; set; } = "";
        public string FromName { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
