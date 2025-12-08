using Microsoft.Extensions.DependencyInjection;
using MinimalAPIValidationDemo.Data;
using MinimalAPIValidationDemo.Models;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<OrderService>();

builder.Services.AddValidation();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference("/", opt => 
{ 
    opt.Title = "Minimal API Validation Demo";
    opt.Theme = ScalarTheme.Mars;
} );

app.MapGet("/orders", (string? query, OrderService orderService) =>
{
    var orders = orderService.GetOrders(query);
    return TypedResults.Ok(orders);
});

app.MapPost("/orders", (OrderDTO order, OrderService orderService) =>
{
    orderService.AddOrder(order);
    return TypedResults.Ok(order);
});

app.MapGet("/jsontotoon", (OrderService orderService) =>
{
    var orders = orderService.JSONToTOON();
    return TypedResults.Ok(orders);
});

app.Run();

