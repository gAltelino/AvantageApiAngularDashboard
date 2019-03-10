using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Avantage.Api.Models;

namespace Avantage.Api.Controllers
{
    [Route("api/[controller]")]
    public class ServerController : Controller
    {

        private readonly ApiContext _ctx;

        public ServerController(ApiContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _ctx.Servers.OrderBy(s => s.Id).ToListAsync();

            return Ok(response);
        }

        [HttpGet("{id}", Name = "GetServer")]
        public async Task<Server> GetServer(int id)
        {
            return await _ctx.Servers.FindAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Server server)
        {
            if (server == null)
            {
                return BadRequest();
            }

            _ctx.Servers.Add(server);
            await _ctx.SaveChangesAsync();

            return CreatedAtRoute("GetServer", new { id = server.Id }, server);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Message(int id, [FromBody] ServerMessage msg)
        {

            var server = await _ctx.Servers.FirstOrDefaultAsync(s => s.Id == id);

            if (server == null)
            {
                return NotFound();
            }

            if (msg.Payload == "activate")
            {
                server.IsOnline = true;
                await _ctx.SaveChangesAsync();
            }

            if (msg.Payload == "deactivate")
            {
                server.IsOnline = false;
                await _ctx.SaveChangesAsync();
            }

            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var server = await _ctx.Servers.FirstOrDefaultAsync(t => t.Id == id);

            if (server == null)
            {
                return NotFound();
            }

            _ctx.Servers.Remove(server);
            await _ctx.SaveChangesAsync();

            return new NoContentResult();
        }


    }

}