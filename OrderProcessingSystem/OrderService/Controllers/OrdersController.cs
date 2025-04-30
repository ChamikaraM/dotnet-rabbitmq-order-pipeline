using Contracts.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace OrderService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderDbContext _db;
    private readonly IPublishEndpoint _publisher;

    public OrdersController(OrderDbContext db, IPublishEndpoint publisher)
    {
        _db = db;
        _publisher = publisher;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var order = new Order
        {
            OrderId = Guid.NewGuid(),
            CustomerEmail = request.CustomerEmail,
            Items = request.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity
            }).ToList()
        };

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        await _publisher.Publish(new OrderCreated(
            order.OrderId,
        order.CustomerEmail,
            order.Items.Select(i => new Contracts.Events.OrderItem(i.ProductId, i.Quantity)).ToList()
        ));

        return Ok(new { order.OrderId });
    }
}
