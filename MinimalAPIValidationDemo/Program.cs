using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MinimalAPIValidationDemo.Models;
using MinimalAPIValidationDemo.Service;
using MinimalAPIValidationDemo.Utility;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<OrderService>();
builder.Services.AddSingleton<ShortenedURLService>();
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

app.MapPost("/generateshorturl", (HttpContext context, ShortenedURLDTO request, ShortenedURLService shortenedURLService) =>
{
    var shorturl = shortenedURLService.GenerateShortUrl(context, request);
    return TypedResults.Ok(shorturl);
});

app.MapGet("/getshorturl", (string code, ShortenedURLService shortenedURLService) =>
{
    var shorturl = shortenedURLService.GetLongUrl(code);
    return TypedResults.Ok(shorturl);
});

app.Run();

