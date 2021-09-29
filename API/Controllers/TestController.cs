using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TestController: BaseController
    {
        [HttpGet("server-error")]
        public ActionResult GetServerError()
        {
            var array = new[] { 1, 2, 4 };
            return Ok(array[10]);
        }

        [HttpGet("not-found")]
        public ActionResult GetNotFound()
        {
            return NotFound("Not found");
        }

        [HttpGet("auth")]
        public ActionResult GetAuth()
        {
            return Unauthorized("Unauthorized");
        }

        [HttpGet("forbid")]
        public ActionResult GetForbid()
        {
            return Forbid();
        }

        [HttpGet("bad-request")]
        public ActionResult GetBadRequest()
        {
            return BadRequest("Bad request");
        }
    }
}
