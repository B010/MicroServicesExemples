using apiPedidos.Models;
using apiPedidos.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddOpenApi();
builder.Services.AddSingleton<PedidoService>();

// Add CORS if needed
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

var pedidoService = app.Services.GetRequiredService<PedidoService>();

// ===== ENDPOINTS GET (READ) =====
app.MapGet("/api/pedidos", async () => await pedidoService.GetTodosPedidos())
    .WithName("GetTodosPedidos")
    .WithOpenApi()
    .Produces<List<Pedido>>(StatusCodes.Status200OK);

app.MapGet("/api/pedidos/{id}", async (int id) =>
    await pedidoService.GetPedidoById(id) is Pedido pedido
        ? Results.Ok(pedido)
        : Results.NotFound("Pedido não encontrado"))
    .WithName("GetPedidoById")
    .WithOpenApi();

app.MapGet("/api/pedidos/cliente/{clienteId}", async (int clienteId) =>
    await pedidoService.GetPedidosPorCliente(clienteId))
    .WithName("GetPedidosPorCliente")
    .WithOpenApi()
    .Produces<List<Pedido>>(StatusCodes.Status200OK);

app.MapGet("/api/pedidos/status/{status}", async (string status) =>
    await pedidoService.GetPedidosPorStatus(status))
    .WithName("GetPedidosPorStatus")
    .WithOpenApi()
    .Produces<List<Pedido>>(StatusCodes.Status200OK);

app.MapGet("/api/pedidos/data/{dataInicio}/{dataFim}", async (DateTime dataInicio, DateTime dataFim) =>
    await pedidoService.GetPedidosPorData(dataInicio, dataFim))
    .WithName("GetPedidosPorData")
    .WithOpenApi()
    .Produces<List<Pedido>>(StatusCodes.Status200OK);

app.MapGet("/api/pedidos/estatisticas", async () =>
    await pedidoService.GetEstatisticas())
    .WithName("GetEstatisticas")
    .WithOpenApi()
    .Produces<Dictionary<string, object>>(StatusCodes.Status200OK);

// ===== ENDPOINTS POST (CREATE) =====
app.MapPost("/api/pedidos", async (Pedido pedido) =>
{
    if (string.IsNullOrWhiteSpace(pedido.ClienteNome))
        return Results.BadRequest("Nome do cliente é obrigatório");

    var novoPedido = await pedidoService.CreatePedido(pedido);
    return Results.Created($"/api/pedidos/{novoPedido.Id}", novoPedido);
})
    .WithName("CreatePedido")
    .WithOpenApi()
    .Produces<Pedido>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest);

// ===== ENDPOINTS PUT (UPDATE) =====
app.MapPut("/api/pedidos/{id}", async (int id, Pedido pedido) =>
{
    var resultado = await pedidoService.UpdatePedido(id, pedido);
    return resultado != null ? Results.Ok(resultado) : Results.NotFound("Pedido não encontrado");
})
    .WithName("UpdatePedido")
    .WithOpenApi()
    .Produces<Pedido>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

app.MapPut("/api/pedidos/{id}/status/{novoStatus}", async (int id, string novoStatus) =>
{
    var resultado = await pedidoService.UpdateStatusPedido(id, novoStatus);
    return resultado ? Results.Ok(new { mensagem = "Status atualizado com sucesso" }) : Results.NotFound("Pedido não encontrado");
})
    .WithName("UpdateStatusPedido")
    .WithOpenApi()
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

// ===== ENDPOINTS DELETE =====
app.MapDelete("/api/pedidos/{id}", async (int id) =>
{
    var resultado = await pedidoService.DeletePedido(id);
    return resultado ? Results.NoContent() : Results.NotFound("Pedido não encontrado");
})
    .WithName("DeletePedido")
    .WithOpenApi()
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status404NotFound);

app.MapDelete("/api/pedidos/{pedidoId}/itens/{itemId}", async (int pedidoId, int itemId) =>
{
    var resultado = await pedidoService.DeleteItemPedido(pedidoId, itemId);
    return resultado ? Results.NoContent() : Results.NotFound("Pedido ou item não encontrado");
})
    .WithName("DeleteItemPedido")
    .WithOpenApi()
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status404NotFound);

app.Run();
