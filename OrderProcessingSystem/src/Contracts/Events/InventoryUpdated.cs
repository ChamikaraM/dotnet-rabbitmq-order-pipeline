namespace Contracts.Events;

public record InventoryUpdated
(
    Guid OrderId,
    bool Success,
    string Message
);
