using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Avantage.Api.Models;
using System.Threading.Tasks;

namespace Avantage.Api.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly ApiContext _ctx;

        public OrderController(ApiContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet("{pageIndex:int}/{pageSize:int}")]
        public IActionResult Get(int pageIndex, int pageSize)
        {
            var data = _ctx.Orders.Include(o => o.Customer).OrderByDescending(c => c.Placed);
            var page = new PaginatedResponse<Order>(data, pageIndex, pageSize);

            var totalCount = data.Count();
            var totalPages = Math.Ceiling((double)totalCount / pageSize);

            var response = new
            {
                Page = page,
                TotalPages = totalPages
            };

            return Ok(response);
        }

        [HttpGet("bystate")]
        public async Task<IActionResult> ByState()
        {
            var orders = await _ctx.Orders.Include(o => o.Customer)
            .GroupBy(r => r.Customer.State)
            .Select(grp => new
            {
                State = grp.Key,
                Total = grp.Sum(x => x.Total)
            }).OrderByDescending(r => r.Total).ToListAsync();

            return Ok(orders);
        }

        [HttpGet("bycustomer/{n}")]
        public async Task<IActionResult> ByCustomer(int n)
        {
            var orders = await _ctx.Orders.Include(o => o.Customer)
            .GroupBy(r => r.Customer.Id)
            .Select(grp => new
            {
                Name = grp.Select(x => x.Customer.Name),
                Total = grp.Sum(x => x.Total)
            }).OrderByDescending(r => r.Total)
            .Take(n)
            .ToListAsync();

            return Ok(orders);
        }

        [HttpGet("getorder/{id}", Name = "GetOrder")]
        public async Task<Order> GetOrder(int id)
        {
            return await _ctx.Orders.Include(o => o.Customer)
            .FirstAsync(o => o.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest();
            }

            _ctx.Orders.Add(order);
            await _ctx.SaveChangesAsync();

            return CreatedAtRoute("GetOrder", new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Order order)
        {
            if (order == null || order.Id != id)
            {
                return BadRequest();
            }

            var updatedOrder = await _ctx.Orders.FirstOrDefaultAsync(c => c.Id == id);

            if (updatedOrder == null)
            {
                return NotFound();
            }

            updatedOrder.Customer = order.Customer;
            updatedOrder.Completed = order.Completed;
            updatedOrder.Total = order.Total;
            updatedOrder.Placed = order.Placed;

            await _ctx.SaveChangesAsync();

            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id){
            var order = await _ctx.Orders.FirstOrDefaultAsync(t => t.Id == id);

            if(order == null){
                return NotFound();
            }

            _ctx.Orders.Remove(order);
            await _ctx.SaveChangesAsync();

            return new NoContentResult();
        }





    }
}