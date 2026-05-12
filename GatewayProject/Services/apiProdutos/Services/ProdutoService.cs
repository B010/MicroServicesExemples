using apiProdutos.Models;

namespace apiProdutos.Services;

public class ProdutoService
{
    // Dados mocados
    private static readonly List<Produto> _produtos = new()
    {
        new Produto
        {
            Id = 1,
            Nome = "Notebook",
            Descricao = "Notebook de alto desempenho com processador i7",
            Preco = 4500.00m,
            Estoque = 10,
            Categoria = "Informática",
            Ativo = true
        },
        new Produto
        {
            Id = 2,
            Nome = "Mouse",
            Descricao = "Mouse sem fio com bateria de longa duração",
            Preco = 89.90m,
            Estoque = 50,
            Categoria = "Periféricos",
            Ativo = true
        },
        new Produto
        {
            Id = 3,
            Nome = "Teclado",
            Descricao = "Teclado mecânico RGB com layout ABNT2",
            Preco = 299.90m,
            Estoque = 25,
            Categoria = "Periféricos",
            Ativo = true
        },
        new Produto
        {
            Id = 4,
            Nome = "Monitor",
            Descricao = "Monitor 27 polegadas 4K com 144Hz",
            Preco = 1200.00m,
            Estoque = 8,
            Categoria = "Monitores",
            Ativo = true
        },
        new Produto
        {
            Id = 5,
            Nome = "Webcam",
            Descricao = "Webcam Full HD com microfone integrado",
            Preco = 199.90m,
            Estoque = 15,
            Categoria = "Periféricos",
            Ativo = true
        },
        new Produto
        {
            Id = 6,
            Nome = "Fone de Ouvido",
            Descricao = "Fone Bluetooth com cancelamento de ruído",
            Preco = 599.90m,
            Estoque = 20,
            Categoria = "Áudio",
            Ativo = true
        }
    };

    // READ - Obter todos os produtos
    public Task<IEnumerable<Produto>> GetTodosProdutos()
    {
        return Task.FromResult(_produtos.AsEnumerable());
    }

    // READ - Obter produto por ID
    public Task<Produto?> GetProdutoById(int id)
    {
        var produto = _produtos.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(produto);
    }

    // READ - Obter produtos por categoria
    public Task<IEnumerable<Produto>> GetProdutosPorCategoria(string categoria)
    {
        var produtos = _produtos.Where(p => p.Categoria.Contains(categoria, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(produtos.AsEnumerable());
    }

    // READ - Buscar produtos por faixa de preço
    public Task<IEnumerable<Produto>> GetProdutosPorPreco(decimal precoMinimo, decimal precoMaximo)
    {
        var produtos = _produtos.Where(p => p.Preco >= precoMinimo && p.Preco <= precoMaximo);
        return Task.FromResult(produtos.AsEnumerable());
    }

    // READ - Buscar produtos ativos
    public Task<IEnumerable<Produto>> GetProdutosAtivos()
    {
        var produtos = _produtos.Where(p => p.Ativo);
        return Task.FromResult(produtos.AsEnumerable());
    }

    // READ - Buscar produtos com estoque disponível
    public Task<IEnumerable<Produto>> GetProdutosComEstoque()
    {
        var produtos = _produtos.Where(p => p.Estoque > 0);
        return Task.FromResult(produtos.AsEnumerable());
    }

    // READ - Buscar produtos por nome
    public Task<IEnumerable<Produto>> GetProdutosPorNome(string nome)
    {
        var produtos = _produtos.Where(p => p.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(produtos.AsEnumerable());
    }

    // READ - Obter todas as categorias
    public Task<IEnumerable<string>> GetCategorias()
    {
        var categorias = _produtos.Select(p => p.Categoria).Distinct();
        return Task.FromResult(categorias.AsEnumerable());
    }

    // READ - Obter estatísticas
    public Task<Dictionary<string, object>> GetEstatisticas()
    {
        var stats = new Dictionary<string, object>
        {
            { "totalProdutos", _produtos.Count },
            { "produtosAtivos", _produtos.Count(p => p.Ativo) },
            { "produtosInativos", _produtos.Count(p => !p.Ativo) },
            { "precoMedio", _produtos.Average(p => p.Preco) },
            { "precoMinimo", _produtos.Min(p => p.Preco) },
            { "precoMaximo", _produtos.Max(p => p.Preco) },
            { "estoqueTotal", _produtos.Sum(p => p.Estoque) },
            { "produtosSemEstoque", _produtos.Count(p => p.Estoque == 0) },
            { "categorias", _produtos.Select(p => p.Categoria).Distinct().ToList() }
        };
        return Task.FromResult(stats);
    }

    // CREATE - Criar novo produto
    public Task<Produto> CreateProduto(Produto produto)
    {
        produto.Id = _produtos.Any() ? _produtos.Max(p => p.Id) + 1 : 1;
        _produtos.Add(produto);
        return Task.FromResult(produto);
    }

    // UPDATE - Atualizar produto existente
    public Task<Produto?> UpdateProduto(int id, Produto produto)
    {
        var produtoExistente = _produtos.FirstOrDefault(p => p.Id == id);
        if (produtoExistente == null)
            return Task.FromResult<Produto?>(null);

        produtoExistente.Nome = produto.Nome;
        produtoExistente.Descricao = produto.Descricao;
        produtoExistente.Preco = produto.Preco;
        produtoExistente.Estoque = produto.Estoque;
        produtoExistente.Categoria = produto.Categoria;
        produtoExistente.Ativo = produto.Ativo;

        return Task.FromResult<Produto?>(produtoExistente);
    }

    // UPDATE - Atualizar estoque
    public Task<bool> AtualizarEstoque(int id, int quantidade)
    {
        var produto = _produtos.FirstOrDefault(p => p.Id == id);
        if (produto == null)
            return Task.FromResult(false);

        produto.Estoque += quantidade;
        return Task.FromResult(true);
    }

    // UPDATE - Desativar produto
    public Task<bool> DesativarProduto(int id)
    {
        var produto = _produtos.FirstOrDefault(p => p.Id == id);
        if (produto == null)
            return Task.FromResult(false);

        produto.Ativo = false;
        return Task.FromResult(true);
    }

    // UPDATE - Ativar produto
    public Task<bool> AtivarProduto(int id)
    {
        var produto = _produtos.FirstOrDefault(p => p.Id == id);
        if (produto == null)
            return Task.FromResult(false);

        produto.Ativo = true;
        return Task.FromResult(true);
    }

    // DELETE - Deletar produto
    public Task<bool> DeleteProduto(int id)
    {
        var produto = _produtos.FirstOrDefault(p => p.Id == id);
        if (produto == null)
            return Task.FromResult(false);

        _produtos.Remove(produto);
        return Task.FromResult(true);
    }
}
