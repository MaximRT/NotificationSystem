namespace NotificationSystem.Services
{
    public interface INotification
    {
        Task Send(string recipient, string header, string body);
    }
}
