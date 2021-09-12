using System;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        public PlatformsController()
        {

        }

        [HttpPost]
        public ActionResult TestInbonudConnection()
        {
            Console.WriteLine("--> Inbound Post # Command Service");
            return Ok("Inbound test of from Platforms Controller");
        }
    }
}