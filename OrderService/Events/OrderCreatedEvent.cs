namespace OrderService.Events
{
    public class OrderCreatedEvent
    {
        public Guid OrderId { get; set; }
        public string CustomerEmail { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
