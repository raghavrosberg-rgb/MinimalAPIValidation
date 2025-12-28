using MinimalAPIValidationDemo.Models;
using MinimalAPIValidationDemo.Utility;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json;

namespace MinimalAPIValidationDemo.Service;

public class OrderService
{
    private readonly List<Order> _orders = [];
    private readonly string jsonordersDataPath = "Data/orders.json";
    private readonly string toonordersDataPath = "Data/orders.toon";
    public OrderService()
    {
        var mockData = File.ReadAllText(jsonordersDataPath);
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
            string updatedJson = JsonSerializer.Serialize(_orders, UrlShortenerSettings.CachedJsonSerializerOptions);
            File.WriteAllText(jsonordersDataPath, updatedJson);
        }
    }

    public string JSONToTOON()
    {
        // Convert JSON to TOON
        // Your JSON string
        string jsonData = File.ReadAllText(jsonordersDataPath);
        var converter = new JsonToToonConverter();
        string toonData = converter.Convert(jsonData);
        // Optionally, save TOON data to a file
        if(File.Exists(toonordersDataPath)) File.Delete(toonordersDataPath);
        File.WriteAllText(toonordersDataPath, toonData);
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

