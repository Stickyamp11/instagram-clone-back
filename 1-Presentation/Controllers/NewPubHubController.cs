using Azure;
using Domain.Entities;
using FluentResults;
using Instagram_Api.Application.Hubs;
using Instagram_Api.Application.Publications.Queries;
using Instagram_Api.Contracts.Hubs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Instagram_Api.Presentation.Controllers
{
    [ApiController]
    [Route("new-pub-hub")]
    public class NewPubHubController : Controller
    {
        private readonly IHubContext<NewPublicationsHub, INewPublicationClient> _hubContext;

        public NewPubHubController(IHubContext<NewPublicationsHub, INewPublicationClient> hubContext)
        {
            _hubContext = hubContext;
        }

        [Route("send")]
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] HubMessageRequest message)
        {
            await _hubContext.Clients.All.ReceiveMessage(message.message);

            return Ok("");
        }
    }
}
