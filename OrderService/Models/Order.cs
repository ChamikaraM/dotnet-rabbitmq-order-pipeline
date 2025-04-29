namespace OrderService.Models
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CustomerEmail { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
