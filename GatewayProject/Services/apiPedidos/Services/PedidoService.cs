using apiPedidos.Models;

namespace apiPedidos.Services;

public class PedidoService
{
    // Dados mocados
    private static readonly List<Pedido> _pedidos = new()
    {
        new Pedido
        {
            Id = 1,
            ClienteId = 101,
            ClienteNome = "João Silva",
            DataPedido = DateTime.Now.AddDays(-5),
            ValorTotal = 250.50m,
            Status = "Entregue",
            Itens = new()
            {
                new ItemPedido { Id = 1, ProdutoId = 1, ProdutoNome = "Notebook", Quantidade = 1, PrecoUnitario = 250.50m, Subtotal = 250.50m }
            }
        },
        new Pedido
        {
            Id = 2,
            ClienteId = 102,
            ClienteNome = "Maria Santos",
            DataPedido = DateTime.Now.AddDays(-2),
            ValorTotal = 150.00m,
            Status = "Processando",
            Itens = new()
            {
                new ItemPedido { Id = 2, ProdutoId = 2, ProdutoNome = "Mouse", Quantidade = 2, PrecoUnitario = 50.00m, Subtotal = 100.00m },
                new ItemPedido { Id = 3, ProdutoId = 3, ProdutoNome = "Teclado", Quantidade = 1, PrecoUnitario = 50.00m, Subtotal = 50.00m }
            }
        },
        new Pedido
        {
            Id = 3,
            ClienteId = 103,
            ClienteNome = "Carlos Oliveira",
            DataPedido = DateTime.Now,
            ValorTotal = 500.00m,
            Status = "Pendente",
            Itens = new()
            {
                new ItemPedido { Id = 4, ProdutoId = 4, ProdutoNome = "Monitor", Quantidade = 1, PrecoUnitario = 500.00m, Subtotal = 500.00m }
            }
        }
    };

    // READ - Obter todos os pedidos
    public Task<IEnumerable<Pedido>> GetTodosPedidos()
    {
        return Task.FromResult(_pedidos.AsEnumerable());
    }

    // READ - Obter pedido por ID
    public Task<Pedido?> GetPedidoById(int id)
    {
        var pedido = _pedidos.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(pedido);
    }

    // READ - Buscar pedidos por cliente
    public Task<IEnumerable<Pedido>> GetPedidosPorCliente(int clienteId)
    {
        var pedidos = _pedidos.Where(p => p.ClienteId == clienteId);
        return Task.FromResult(pedidos.AsEnumerable());
    }

    // READ - Buscar pedidos por status
    public Task<IEnumerable<Pedido>> GetPedidosPorStatus(string status)
    {
        var pedidos = _pedidos.Where(p => p.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(pedidos.AsEnumerable());
    }

    // READ - Buscar pedidos por intervalo de datas
    public Task<IEnumerable<Pedido>> GetPedidosPorData(DateTime dataInicio, DateTime dataFim)
    {
        var pedidos = _pedidos.Where(p => p.DataPedido >= dataInicio && p.DataPedido <= dataFim);
        return Task.FromResult(pedidos.AsEnumerable());
    }

    // READ - Obter estatísticas de pedidos
    public Task<Dictionary<string, object>> GetEstatisticas()
    {
        var stats = new Dictionary<string, object>
        {
            { "totalPedidos", _pedidos.Count },
            { "valorTotal", _pedidos.Sum(p => p.ValorTotal) },
            { "valorMedio", _pedidos.Count > 0 ? _pedidos.Average(p => p.ValorTotal) : 0 },
            { "pedidosPorStatus", _pedidos.GroupBy(p => p.Status)
                .ToDictionary(g => g.Key, g => g.Count()) },
            { "totalItens", _pedidos.Sum(p => p.Itens.Count) }
        };
        return Task.FromResult(stats);
    }

    // CREATE - Criar novo pedido
    public Task<Pedido> CreatePedido(Pedido pedido)
    {
        pedido.Id = _pedidos.Any() ? _pedidos.Max(p => p.Id) + 1 : 1;
        pedido.DataPedido = DateTime.Now;

        if (pedido.Itens != null && pedido.Itens.Any())
        {
            int itemId = 1;
            foreach (var item in pedido.Itens)
            {
                item.Id = itemId++;
            }
        }

        _pedidos.Add(pedido);
        return Task.FromResult(pedido);
    }

    // UPDATE - Atualizar pedido existente
    public Task<Pedido?> UpdatePedido(int id, Pedido pedido)
    {
        var pedidoExistente = _pedidos.FirstOrDefault(p => p.Id == id);
        if (pedidoExistente == null)
            return Task.FromResult<Pedido?>(null);

        pedidoExistente.ClienteId = pedido.ClienteId;
        pedidoExistente.ClienteNome = pedido.ClienteNome;
        pedidoExistente.ValorTotal = pedido.ValorTotal;
        pedidoExistente.Status = pedido.Status;

        if (pedido.Itens != null)
        {
            int itemId = pedidoExistente.Itens.Any() ? pedidoExistente.Itens.Max(i => i.Id) + 1 : 1;
            foreach (var item in pedido.Itens)
            {
                if (item.Id == 0)
                    item.Id = itemId++;
            }
            pedidoExistente.Itens = pedido.Itens;
        }

        return Task.FromResult<Pedido?>(pedidoExistente);
    }

    // UPDATE - Atualizar status do pedido
    public Task<bool> UpdateStatusPedido(int id, string novoStatus)
    {
        var pedido = _pedidos.FirstOrDefault(p => p.Id == id);
        if (pedido == null)
            return Task.FromResult(false);

        pedido.Status = novoStatus;
        return Task.FromResult(true);
    }

    // DELETE - Deletar pedido
    public Task<bool> DeletePedido(int id)
    {
        var pedido = _pedidos.FirstOrDefault(p => p.Id == id);
        if (pedido == null)
            return Task.FromResult(false);

        _pedidos.Remove(pedido);
        return Task.FromResult(true);
    }

    // DELETE - Deletar item do pedido
    public Task<bool> DeleteItemPedido(int pedidoId, int itemId)
    {
        var pedido = _pedidos.FirstOrDefault(p => p.Id == pedidoId);
        if (pedido == null)
            return Task.FromResult(false);

        var item = pedido.Itens.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
            return Task.FromResult(false);

        pedido.Itens.Remove(item);
        return Task.FromResult(true);
    }
}
