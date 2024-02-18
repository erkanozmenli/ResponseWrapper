# .NET 6 API Response Wrapper Middleware

A Simple .NET 6 API response wrapper including exception handler. Download and import Postman collection from repository to test it out.

Additionally the following code block has been added to Program.cs to prevent 204 no-content issue.

```csharp
builder.Services.AddControllers(opt =>
    opt.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>()
);
```

