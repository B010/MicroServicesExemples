var builder = WebApplication.CreateBuilder(args);

// Registrar YARP Reverse Proxy
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Add CORS para permitir requisições cross-origin
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader();
    });
});

var app = builder.Build();

// Use CORS
app.UseCors("AllowAll");

// Use HTTPS Redirection
app.UseHttpsRedirection();

// Endpoint health check
app.MapGet("/health", () => new { status = "Gateway funcionando! ✅", timestamp = DateTime.UtcNow })
    .WithName("HealthCheck")
    .WithOpenApi();

// Endpoint info do gateway
app.MapGet("/gateway/info", () => new
{
    gateway = "YARP Reverse Proxy",
    version = "1.0",
    services = new[]
    {
        new { name = "apiPedidos", url = "http://localhost:5026", pattern = "/api/pedidos" },
        new { name = "apiProdutos", url = "http://localhost:5004", pattern = "/api/produtos" }
    },
    timestamp = DateTime.UtcNow
})
.WithName("GatewayInfo")
.WithOpenApi();

// Mapear YARP Reverse Proxy
app.MapReverseProxy();

app.Run();

