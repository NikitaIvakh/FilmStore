using System.Diagnostics;

namespace FilmStore.Messages
{
    public class DebugNotificationService : INotificationService
    {
        public Task SendConfirmationCodeAsync(string cellPhone, int code)
        {
            Debug.WriteLine("Cell phone: {0}, code: {1:0000}.", cellPhone, code);
            return Task.CompletedTask;
        }

        public Task StartProcessAsync(Order order)
        {
            StartProcess(order);
            return Task.CompletedTask;
        }

        private static void StartProcess(Order order)
        {
            Debug.WriteLine("Order ID {0}", order.Id);
            Debug.WriteLine("Delivery: {0}", (object)order.Delivery.Description);
            Debug.WriteLine("Payment: {0}", (object)order.Payment.Description);
        }
    }
}