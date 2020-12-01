using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Farmatiko.Controllers
{
    [ApiController]
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }
        [Route("/Error")]
        public IActionResult Error()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var statusCode = exception.Error.GetType().Name switch
            {
                "ArgumentException" => HttpStatusCode.BadRequest,
                "Exception" => HttpStatusCode.InternalServerError,
                /*"NotFoundResult" => HttpStatusCode.NotFound,*/
                _ => HttpStatusCode.ServiceUnavailable
            };
            _logger.LogInformation(statusCode.ToString() + " " + exception.ToString());
            return Problem(detail: exception.Error.Message, statusCode: (int)statusCode);
        }
    }
}
