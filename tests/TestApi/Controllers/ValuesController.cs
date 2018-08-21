using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Task<string> Get(int id)
        {
            return Task.FromResult("value");
        }

        // POST api/values
        [HttpPost]
        public Task Post([FromBody] string value)
        {
            return Task.CompletedTask;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public Task Put(int id, [FromBody] string value)
        {
            return Task.CompletedTask;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public Task Delete(int id)
        {
            return Task.CompletedTask;
        }
    }
}
