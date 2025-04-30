namespace Contracts.Events;

public record OrderCreated
(
    Guid OrderId,
    string CustomerEmail,
    List<OrderItem> Items
);

public record OrderItem
(
    Guid ProductId,
    int Quantity
);
