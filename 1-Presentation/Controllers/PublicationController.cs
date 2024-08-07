using Microsoft.AspNetCore.Mvc;
using Instagram_Api.Application.Auth.Models;
using FluentResults;
using MediatR;
using Instagram_Api.Application.Auth.Commands;
using Domain.Entities;
using Instagram_Api.Contracts.Publications;
using Instagram_Api.Application.Publications.Queries;
using Instagram_Api.Application.Publications.Commands;
using Microsoft.AspNetCore.Authorization;
using Instagram_Api.Application.Publications.Models;

namespace Instagram_Api.Presentation.Controllers
{
    [ApiController]
    [Route("publication")]
    public class PublicationController : Controller
    {
        private readonly ISender _mediator;

        public PublicationController(ISender mediator)
        {
            _mediator = mediator;
        }

        [Route("{userGuid}")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPublications([FromRoute] string userGuid, [FromQuery] string range)
        {
            var command = new PublicationsByUserGuidQuery(userGuid, range);
            Result<PaginatedList<Publication>> response = await _mediator.Send(command);

            if (response.IsSuccess)
            {
                return Ok(response.Value.Items);
            }

            var firstError = response.Errors[0];
            return Problem(statusCode: StatusCodes.Status400BadRequest, detail: firstError.Message);

        }

        [Route("{userGuid}")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePublication([FromBody] PublicationRequest publicationRequest, [FromRoute] string userGuid)
        {

            var publication = new Publication()
            {
                Title = publicationRequest.Title,
                Description = publicationRequest.Description,
                //userGuid = userGuid
            };
            var command = new AddPublicationCommand(publication, userGuid);
            Result<int> response = await _mediator.Send(command);

            if (response.IsSuccess)
            {
                return Created();
            }

            var firstError = response.Errors[0];
            return Problem(statusCode: StatusCodes.Status400BadRequest, detail: firstError.Message);

        }
    }
}
