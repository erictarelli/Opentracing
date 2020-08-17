using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Example.A.DB;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Example.A.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExampleAController : ControllerBase
    {
        private readonly JaegerExampleContext _db;

        public ExampleAController(JaegerExampleContext db)
        {
            _db = db;
        }

        // GET: api/<TestController>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var alumnos = await (from s in _db.TblAlumno select s).ToListAsync();
            return Ok(alumnos);
        }

        // GET api/<TestController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {

            var alumnos = await (from s in _db.TblAlumno where s.Id == id select s).ToListAsync();
            return Ok(alumnos);
        }

        // POST api/<TestController>
        [HttpPost]
        public async Task Post([FromBody] TblAlumno alumno)
        {
            if (string.IsNullOrEmpty(alumno.Email))
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync($"https://localhost:5003/api/exampleb?nombre={alumno.Nombre}&apellido={alumno.Apellido}");
                alumno.Email = await response.Content.ReadAsStringAsync();
            }
            _db.Set<TblAlumno>().AddRange(alumno);
            await _db.SaveChangesAsync();
        }

        // PUT api/<TestController>/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] TblAlumno alumno)
        {

            var alumnoToUpdate = await (from s in _db.TblAlumno where s.Id == id select s).FirstOrDefaultAsync();

            if (alumnoToUpdate != null)
            {
                alumno.Id = id;
                //_db.Entry(alumnoToUpdate).CurrentValues.SetValues(alumno);
                _db.Update(alumnoToUpdate).CurrentValues.SetValues(alumno);
                await _db.SaveChangesAsync();
            }

        }

        // DELETE api/<TestController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var alumnoToUpdate = await (from s in _db.TblAlumno where s.Id == id select s).FirstOrDefaultAsync();
            if (alumnoToUpdate != null)
            {
                _db.TblAlumno.Remove(alumnoToUpdate);
                await _db.SaveChangesAsync();
            }
        }
    }
}
