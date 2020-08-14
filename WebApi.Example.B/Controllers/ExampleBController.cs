using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Example.B.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExampleBController : ControllerBase
    {
        // GET: api/<TestController>
        [HttpGet]
        public async Task<ActionResult> Get(string nombre, string apellido)
        {
            return Ok($"{nombre}.{apellido}@gmail.com");
        }
    }
}
