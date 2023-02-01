using Microsoft.AspNetCore.Mvc;

namespace M33tingClub.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult Get()
		=> Ok();
}