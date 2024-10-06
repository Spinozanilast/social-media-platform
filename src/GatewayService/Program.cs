var builder = WebApplication.CreateBuilder(args);
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
var app = builder.Build();
app.UseCors(policyBuilder => policyBuilder.AllowAnyOrigin());
app.MapReverseProxy();
app.Run();