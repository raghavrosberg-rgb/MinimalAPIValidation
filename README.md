# Minimal API Validation Demo

This repository demonstrates model validation in Minimal APIs using .NET 10+. 
The project includes examples for validating requests using built-in 
[DataAnnotations attributes](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=net-10.0),
custom [validation attributes](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.validationattribute?view=net-10.0), 
and the [IValidatableObject interface](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.ivalidatableobject?view=net-9.0)
using the new [validation support for minimal APIs](https://learn.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-10.0?view=aspnetcore-10.0#validation-support-in-minimal-apis) 
in .NET 10+.

## 🎥 Video Tutorial

Watch the step-by-step walkthrough on YouTube:

[![Watch the video](https://img.youtube.com/vi/oGbmxdXoMP0/default.jpg)](https://youtu.be/oGbmxdXoMP0)

## 🚀 Prerequisites

- .NET 10 Preview 4 or later SDK
- Visual Studio 2022 Preview or Visual Studio Code with the latest pre-release C# extension, or any other IDE that supports .NET 10+ development

## 🛠️ Configuration

Validation support and the Scalar testing UI are already configured in this repo. To enable validation in your
project, follow the steps below.

1. Enable source-generated validation. Add the following to your .csproj file:
    ```xml
    <PropertyGroup>
      <InterceptorsNamespaces>$(InterceptorsNamespaces);Microsoft.AspNetCore.Http.Validation.Generated</InterceptorsNamespaces>
    </PropertyGroup>
    ```
2. Register the validation services in Program.cs:
    ```csharp
    var builder = WebApplication.CreateBuilder(args);
    
    // Enable validation for minimal APIs
    builder.Services.AddValidation();
    
    var app = builder.Build();
    ```
3. To use Scalar for API documentation and testing, add the following NuGet packages:
    - Microsoft.AspNetCore.OpenApi
    - Scalar.AspNetCore  
    
    Then configure the following in your `Program.cs`:

    ```csharp
    var builder = WebApplication.CreateBuilder(args);

    // generate documentation
    builder.Services.AddOpenApi(); 

    var app = builder.Build();

    app.MapOpenApi();
    // scalar will be at /scalar/v1 by default. Alternatively, you can specify a different path.
    app.MapScalarApiReference();
   ```

## ✅ Model Validation Examples
To demonstrate the new support for model validation in minimal APIs, this project uses an 
[OrderDTO](./MinimalAPIValidationDemo/Models/OrderDTO.cs) class that is validated when used in the
endpoints defined in the [Program.cs](./MinimalAPIValidationDemo/Program.cs). The examples below 
are taken from this class.

### DataAnnotation
```csharp
[Required]
[Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
public int Quantity { get; set; }
```
With validation enabled, any minimal API parameters that contain validation attributes will be
automatically validated before the API endpoint is invoked. If validation fails, the API will 
not be called.

This property uses the `[Required]` attribute to validate that there is a non-null value provided 
on incoming requests. Additionally, the `[Range]` attribute validates that the value is at least 1, 
with a custom error message provided if the quantity is 0 or less.

### Custom Validation Attribute
```csharp
[Display(Name = "Delivery Date")]
[BusinessDay] 
public DateTime DeliveryDate { get; set; }
```
```csharp
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class BusinessDayAttribute : ValidationAttribute
{
   public BusinessDayAttribute()
       => ErrorMessage = "The {0} must fall on a business day (Monday–Friday).";

   public override bool IsValid(object? value)
   {
       // Let [Required] handle nulls if you need them.
       if (value is DateTime dt)
       {
           return dt.DayOfWeek is not DayOfWeek.Saturday
               && dt.DayOfWeek is not DayOfWeek.Sunday;
       }
       return true;
   }
}
```
The `BusinessDayAttribute` is a custom validation attribute which is validated in the same way as the
validation attributes from `System.ComponentModel.DataAnnotations` demonstrated above. 

The `IsValid` method in this example returns `true` for days Monday through Friday, and `false` for 
Saturday and Sunday. If the method returns `false`, the API endpoint will not be invoked and the 
error message will be returned to the client.

### `IValidatableObject` Interface
```csharp
public class OrderDTO : IValidatableObject
{
   // Other properties...

   public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
   {
      // Example rule: total must not exceed $10,000
      if (UnitPrice * Quantity > 10_000m)
      {
          yield return new ValidationResult(
              "Order total cannot exceed $10,000.",
              new[] { nameof(UnitPrice), nameof(Quantity) }
          );
      }
   }
}
```
If a minimal API parameter implements the `IValidatableObject` interface, the `Validate` method
will be called before invoking that API endpoint to check for validation errors. If one or more errors
are returned from this method, the endpoint will not be invoked and the errors will be returned
to the client.

In this example, the `Validate` method checks if the total order amount exceeds $10,000 to prevent
orders that are too large to be placed via the API.

## 📝 API Docs & Interactive UI

The API documentation and built-in testing UI are preconfigured. To use:

- Run the application (`dotnet run` or press F5 in Visual Studio).

- Navigate to https://localhost:7272 to launch the Scalar interface.

## 🔗 Links & Resources

[.NET DataAnnotations Documentation](https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-10.0)

[Minimal APIs in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-10.0)


