var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.Use(async (context, next) =>
{
    Console.WriteLine("Logic before the executing the next delegate in the Use method.");
    await next.Invoke();
    Console.WriteLine("Logic after the executing the next delegate in the Use method.");
});

app.Map("/usingmapbranch", builder =>
{
    builder.Use(async (context, next) =>
    {
        Console.WriteLine("Map branch logic before the next delegate in the Use method.");
        await next.Invoke();
        Console.WriteLine("Map branch logic after the next delegate in the Use method.");
    });
    builder.Run(async context =>
    {
        Console.WriteLine($"Map branch response to the client in the Run method.");
        await context.Response.WriteAsync("Hello from the map branch.");
    });
});

app.MapWhen(context => context.Request.Query.ContainsKey("testquerystring"), builder =>
{
    builder.Run(async context =>
    {
        await context.Response.WriteAsync("Hello from MapWhen branch.");
    });
});

app.Run(async context =>
{
    Console.WriteLine("Writing the response to the client in the Run method.");
    await context.Response.WriteAsync("Hello from the middleware component.");
});



app.MapControllers();

app.Run();
