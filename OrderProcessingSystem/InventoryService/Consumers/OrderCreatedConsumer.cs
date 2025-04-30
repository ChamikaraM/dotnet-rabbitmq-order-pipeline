using Contracts.Events;
using MassTransit;

namespace InventoryService;

public class OrderCreatedConsumer : IConsumer<OrderCreated>
{
    private readonly InventoryDbContext _db;
    private readonly IPublishEndpoint _publisher;

    public OrderCreatedConsumer(InventoryDbContext db, IPublishEndpoint publisher)
    {
        _db = db;
        _publisher = publisher;
    }

    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        var order = context.Message;

        bool success = true;
        foreach (var item in order.Items)
        {
            var stock = await _db.Stocks.FindAsync(item.ProductId);
            if (stock == null || stock.Quantity < item.Quantity)
            {
                success = false;
                break;
            }
        }

        if (success)
        {
            foreach (var item in order.Items)
            {
                var stock = await _db.Stocks.FindAsync(item.ProductId);
                stock!.Quantity -= item.Quantity;
            }
            await _db.SaveChangesAsync();
        }

        await _publisher.Publish(new InventoryUpdated(
            order.OrderId,
            success,
            success ? "Inventory updated successfully." : "Insufficient stock."
        ));
    }
}
