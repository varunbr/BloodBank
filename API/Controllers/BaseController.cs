using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ServiceFilter(typeof(LogActivity))]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
    }
}
