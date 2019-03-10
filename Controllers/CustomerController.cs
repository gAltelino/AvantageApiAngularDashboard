using Avantage.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Avantage.Api.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private readonly ApiContext _ctx;

        public CustomerController(ApiContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet("{pageIndex:int}/{pageSize:int}")]
        public IActionResult GetPagination(int pageIndex, int pageSize)
        {

            var data = _ctx.Customers.OrderBy(c => c.Id);
            var page = new PaginatedResponse<Customer>(data, pageIndex, pageSize);

            var totalCount = data.Count();
            var totalPages = Math.Ceiling((double)totalCount / pageSize);

            var resp = new
            {
                Page = page,
                TotalPages = totalPages
            };

            return Ok(resp);
        }

        [HttpGet("{id}", Name = "GetCustomer")]
        public async Task<Customer> GetCustomer(int id)
        {
            return await _ctx.Customers.FindAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Customer customer)
        {
            if (customer == null)
            {
                return BadRequest();
            }
            else
            {
                _ctx.Customers.Add(customer);
                await _ctx.SaveChangesAsync();

                return CreatedAtRoute("GetCustomer", new { id = customer.Id }, customer);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Customer customer)
        {
            if (customer == null || customer.Id != id)
            {
                return BadRequest();
            }

            var updatedCustomer = _ctx.Customers.FirstOrDefault(c => c.Id == id);

            if (updatedCustomer == null)
            {
                return NotFound();
            }

            updatedCustomer.Email = customer.Email;
            updatedCustomer.Name = customer.Name;
            updatedCustomer.State = customer.State;

            await _ctx.SaveChangesAsync();

            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = _ctx.Customers.FirstOrDefault(t => t.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            _ctx.Customers.Remove(customer);
            await _ctx.SaveChangesAsync();

            return new NoContentResult();
        }


    }
}