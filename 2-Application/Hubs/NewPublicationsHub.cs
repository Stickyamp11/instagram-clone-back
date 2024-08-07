using Microsoft.AspNetCore.SignalR;

namespace Instagram_Api.Application.Hubs
{
    public sealed class NewPublicationsHub : Hub<INewPublicationClient>
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.ReceiveMessage($"{Context.ConnectionId}");
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.ReceiveMessage(message);
        }
    }
}
