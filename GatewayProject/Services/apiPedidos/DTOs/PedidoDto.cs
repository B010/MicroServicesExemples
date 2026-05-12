namespace apiPedidos.DTOs;

public class CreatePedidoDto
{
    public int ClienteId { get; set; }
    public string ClienteNome { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }
    public string Status { get; set; } = "Pendente";
    public List<CreateItemPedidoDto> Itens { get; set; } = new();
}

public class CreateItemPedidoDto
{
    public int ProdutoId { get; set; }
    public string ProdutoNome { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Subtotal { get; set; }
}

public class UpdatePedidoDto
{
    public int ClienteId { get; set; }
    public string ClienteNome { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<CreateItemPedidoDto> Itens { get; set; } = new();
}

public class PedidoDto
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string ClienteNome { get; set; } = string.Empty;
    public DateTime DataPedido { get; set; }
    public decimal ValorTotal { get; set; }
    public string Status { get; set; } = "Pendente";
    public List<CreateItemPedidoDto> Itens { get; set; } = new();
}
