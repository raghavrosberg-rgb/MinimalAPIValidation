using System.ComponentModel.DataAnnotations;

namespace MinimalAPIValidationDemo.Models;
public record Order 
{
   
    public required string CustomerName { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }
    
   public DateTime OrderDate { get; private set; } = DateTime.UtcNow;
         
    public DateTime DeliveryDate { get; set; } 

    // Read-only computed property
    public decimal TotalPrice => UnitPrice * Quantity;
}