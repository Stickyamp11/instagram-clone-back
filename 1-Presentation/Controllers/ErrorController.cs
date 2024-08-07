using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Instagram_Api.Presentation.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/error")]
        [HttpGet]
        public IActionResult Error()
        {
            Exception exception = HttpContext.Features.Get<IExceptionHandlerFeature>().Error;
            return Problem(
                title: exception?.Message,
                statusCode: 500
                );
        }
    }

}
