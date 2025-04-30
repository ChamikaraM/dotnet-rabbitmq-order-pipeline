using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace InventoryService;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }
    public DbSet<Stock> Stocks => Set<Stock>();
}

public class Stock
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
