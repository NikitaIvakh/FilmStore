namespace FilmStore.Messages
{
    public interface INotificationService
    {
        public Task SendConfirmationCodeAsync(string cellPhone, int code);

        public Task StartProcessAsync(Order order);
    }
}