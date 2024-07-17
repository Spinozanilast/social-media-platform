var builder = WebApplication.CreateBuilder(args);
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddCors(options =>
{
    options.AddPolicy("allowAny", builder =>
    {
        builder.AllowAnyOrigin();
    });
});
var app = builder.Build();
app.UseCors();
app.MapReverseProxy();
app.Run();