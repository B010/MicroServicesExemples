using apiProdutos.Models;
using apiProdutos.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddOpenApi();
builder.Services.AddSingleton<ProdutoService>();

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

var produtoService = app.Services.GetRequiredService<ProdutoService>();

// ===== ENDPOINTS GET (READ) =====
app.MapGet("/api/produtos", async () => await produtoService.GetTodosProdutos())
    .WithName("GetTodosProdutos")
    .WithOpenApi()
    .Produces<List<Produto>>(StatusCodes.Status200OK);

app.MapGet("/api/produtos/{id}", async (int id) =>
    await produtoService.GetProdutoById(id) is Produto produto
        ? Results.Ok(produto)
        : Results.NotFound("Produto não encontrado"))
    .WithName("GetProdutoById")
    .WithOpenApi();

app.MapGet("/api/produtos/categoria/{categoria}", async (string categoria) =>
    await produtoService.GetProdutosPorCategoria(categoria))
    .WithName("GetProdutosPorCategoria")
    .WithOpenApi()
    .Produces<List<Produto>>(StatusCodes.Status200OK);

app.MapGet("/api/produtos/preco/{precoMinimo}/{precoMaximo}", async (decimal precoMinimo, decimal precoMaximo) =>
    await produtoService.GetProdutosPorPreco(precoMinimo, precoMaximo))
    .WithName("GetProdutosPorPreco")
    .WithOpenApi()
    .Produces<List<Produto>>(StatusCodes.Status200OK);

app.MapGet("/api/produtos/nome/{nome}", async (string nome) =>
    await produtoService.GetProdutosPorNome(nome))
    .WithName("GetProdutosPorNome")
    .WithOpenApi()
    .Produces<List<Produto>>(StatusCodes.Status200OK);

app.MapGet("/api/produtos/ativos", async () =>
    await produtoService.GetProdutosAtivos())
    .WithName("GetProdutosAtivos")
    .WithOpenApi()
    .Produces<List<Produto>>(StatusCodes.Status200OK);

app.MapGet("/api/produtos/com-estoque", async () =>
    await produtoService.GetProdutosComEstoque())
    .WithName("GetProdutosComEstoque")
    .WithOpenApi()
    .Produces<List<Produto>>(StatusCodes.Status200OK);

app.MapGet("/api/categorias", async () =>
    await produtoService.GetCategorias())
    .WithName("GetCategorias")
    .WithOpenApi()
    .Produces<List<string>>(StatusCodes.Status200OK);

app.MapGet("/api/produtos/estatisticas", async () =>
    await produtoService.GetEstatisticas())
    .WithName("GetEstatisticas")
    .WithOpenApi()
    .Produces<Dictionary<string, object>>(StatusCodes.Status200OK);

// ===== ENDPOINTS POST (CREATE) =====
app.MapPost("/api/produtos", async (Produto produto) =>
{
    if (string.IsNullOrWhiteSpace(produto.Nome))
        return Results.BadRequest("Nome do produto é obrigatório");

    var novoProduto = await produtoService.CreateProduto(produto);
    return Results.Created($"/api/produtos/{novoProduto.Id}", novoProduto);
})
    .WithName("CreateProduto")
    .WithOpenApi()
    .Produces<Produto>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest);

// ===== ENDPOINTS PUT (UPDATE) =====
app.MapPut("/api/produtos/{id}", async (int id, Produto produto) =>
{
    var resultado = await produtoService.UpdateProduto(id, produto);
    return resultado != null ? Results.Ok(resultado) : Results.NotFound("Produto não encontrado");
})
    .WithName("UpdateProduto")
    .WithOpenApi()
    .Produces<Produto>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

app.MapPut("/api/produtos/{id}/estoque/{quantidade}", async (int id, int quantidade) =>
{
    var resultado = await produtoService.AtualizarEstoque(id, quantidade);
    return resultado ? Results.Ok(new { mensagem = "Estoque atualizado com sucesso" }) : Results.NotFound("Produto não encontrado");
})
    .WithName("AtualizarEstoque")
    .WithOpenApi()
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

app.MapPut("/api/produtos/{id}/desativar", async (int id) =>
{
    var resultado = await produtoService.DesativarProduto(id);
    return resultado ? Results.Ok(new { mensagem = "Produto desativado com sucesso" }) : Results.NotFound("Produto não encontrado");
})
    .WithName("DesativarProduto")
    .WithOpenApi()
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

app.MapPut("/api/produtos/{id}/ativar", async (int id) =>
{
    var resultado = await produtoService.AtivarProduto(id);
    return resultado ? Results.Ok(new { mensagem = "Produto ativado com sucesso" }) : Results.NotFound("Produto não encontrado");
})
    .WithName("AtivarProduto")
    .WithOpenApi()
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

// ===== ENDPOINTS DELETE =====
app.MapDelete("/api/produtos/{id}", async (int id) =>
{
    var resultado = await produtoService.DeleteProduto(id);
    return resultado ? Results.NoContent() : Results.NotFound("Produto não encontrado");
})
    .WithName("DeleteProduto")
    .WithOpenApi()
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status404NotFound);

app.Run();
