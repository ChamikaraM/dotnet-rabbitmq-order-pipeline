using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OrderService.Data;
using OrderService.Events;
using OrderService.Models;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderController(OrderDbContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var orderEvent = new OrderCreatedEvent
            {
                OrderId = order.Id,
                CustomerEmail = order.CustomerEmail,
                TotalAmount = order.TotalAmount
            };

            await _publishEndpoint.Publish(orderEvent);

            return Ok(new { Message = "Order created successfully", order.Id });
        }
    }
}
