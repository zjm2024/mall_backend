using Microsoft.AspNetCore.SignalR;

namespace shopmallService.Hubs
{
    public class OrderNotificationHub : Hub
    {
        // 发送订单处理失败通知
        public async Task SendOrderFailureNotification(string orderNumber, string errorMessage)
        {
      
            await Clients.All.SendAsync("ReceiveOrderFailure", orderNumber, errorMessage);
        }

        // 发送给特定用户的订单失败通知
        public async Task SendOrderFailureToUser(string userId, string orderNumber, string errorMessage)
        {
            await Clients.User(userId).SendAsync("ReceivePersonalOrderFailure", orderNumber, errorMessage);
        }

        // 发送订单处理成功通知
        public async Task SendOrderSuccessNotification(string orderNumber, string message)
        {
            await Clients.All.SendAsync("ReceiveOrderSuccess", orderNumber, message);
        }
    }
}
