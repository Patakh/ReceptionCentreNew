namespace ReceptionCentreNew.Models.Service;

public interface ISmsSender
{
    Task SendSmsAsync(string phoneNumber, string message);
}

public class SmsService : ISmsSender
{
    public Task SendSmsAsync(string phoneNumber, string message) =>
          Task.CompletedTask;
}