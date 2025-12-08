using MinimalAPIValidationDemo.Models;
using MinimalAPIValidationDemo.Utility;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json;

namespace MinimalAPIValidationDemo.Data;

public class OrderService
{
    private readonly List<Order> _orders = [];

    public OrderService()
    {
        var mockData = File.ReadAllText("Data/orders.json");
        _orders = JsonSerializer.Deserialize<List<Order>>(mockData, JsonSerializerOptions.Web) ?? [];
    }

    public List<Order> GetOrders(string? searchTerm = null)
    {
        return string.IsNullOrWhiteSpace(searchTerm)
            ? _orders
            : [.. _orders.Where(o => $"{o.CustomerName}".Contains(searchTerm, StringComparison.OrdinalIgnoreCase))];
    }

    public void AddOrder(OrderDTO order)
    {
        // Map OrderDTO to Order
        var newOrder = new Order
        {
            CustomerName = order.CustomerName,
            UnitPrice = order.UnitPrice,
            Quantity = order.Quantity,
            DeliveryDate = order.DeliveryDate
        };

        if (!_orders.Contains(newOrder))
        {
            _orders.Add(newOrder);
        }
    }

    public string JSONToTOON()
    {
        // Convert JSON to TOON
        // Your JSON string
        string jsonData = File.ReadAllText("Data/orders.json");
        var converter = new JsonToToonConverter();
        string toonData = converter.Convert(jsonData);
        // Optionally, save TOON data to a file
        if(File.Exists("Data/orders.toon")) File.Delete("Data/orders.toon");
        File.WriteAllText("Data/orders.toon", toonData);
        StringBuilder sb = new();

        // Count tokens saved
        int jsonTokens = TokenCounter.Count(jsonData);
        int toonTokens = TokenCounter.Count(toonData);
        int saved = jsonTokens - toonTokens;
        double percentage = (saved / (double)jsonTokens) * 100;

        sb.AppendLine($"\nJSON tokens: {jsonTokens}");
        sb.AppendLine($"TOON tokens: {toonTokens}");
        sb.AppendLine($"Saved: {saved} tokens ({percentage:F1}%)");
        return sb.ToString();
    }
}

