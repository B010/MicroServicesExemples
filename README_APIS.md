# APIs com Dados Mocados

## Descrição
Este projeto contém duas APIs ASP.NET Core (apiPedidos e apiProdutos) com dados mocados (fake data) sem conexão com banco de dados. Implementam operações completas de CRUD (Create, Read, Update, Delete) com dados armazenados em memória.

## Estrutura

### apiPedidos
- **Porta**: 5026
- **Modelos**: Pedido, ItemPedido
- **Dados**: 3 pedidos mocados
- **Métodos CRUD Completos**: ✅

#### Endpoints GET (Read)
- `GET /api/pedidos` - Listar todos os pedidos
- `GET /api/pedidos/{id}` - Obter pedido por ID
- `GET /api/pedidos/cliente/{clienteId}` - Buscar pedidos por cliente
- `GET /api/pedidos/status/{status}` - Buscar pedidos por status
- `GET /api/pedidos/data/{dataInicio}/{dataFim}` - Buscar pedidos por intervalo de datas
- `GET /api/pedidos/estatisticas` - Obter estatísticas

#### Endpoints POST (Create)
- `POST /api/pedidos` - Criar novo pedido

#### Endpoints PUT (Update)
- `PUT /api/pedidos/{id}` - Atualizar pedido completo
- `PUT /api/pedidos/{id}/status/{novoStatus}` - Atualizar apenas o status

#### Endpoints DELETE
- `DELETE /api/pedidos/{id}` - Deletar pedido
- `DELETE /api/pedidos/{pedidoId}/itens/{itemId}` - Deletar item específico do pedido

---

### apiProdutos
- **Porta**: 5004
- **Modelos**: Produto
- **Dados**: 6 produtos mocados
- **Métodos CRUD Completos**: ✅

#### Endpoints GET (Read)
- `GET /api/produtos` - Listar todos os produtos
- `GET /api/produtos/{id}` - Obter produto por ID
- `GET /api/produtos/categoria/{categoria}` - Filtrar por categoria
- `GET /api/produtos/preco/{precoMinimo}/{precoMaximo}` - Filtrar por faixa de preço
- `GET /api/produtos/nome/{nome}` - Buscar por nome
- `GET /api/produtos/ativos` - Listar apenas produtos ativos
- `GET /api/produtos/com-estoque` - Listar apenas produtos com estoque
- `GET /api/categorias` - Obter todas as categorias
- `GET /api/produtos/estatisticas` - Obter estatísticas

#### Endpoints POST (Create)
- `POST /api/produtos` - Criar novo produto

#### Endpoints PUT (Update)
- `PUT /api/produtos/{id}` - Atualizar produto completo
- `PUT /api/produtos/{id}/estoque/{quantidade}` - Atualizar estoque (pode ser negativo)
- `PUT /api/produtos/{id}/desativar` - Desativar produto
- `PUT /api/produtos/{id}/ativar` - Ativar produto

#### Endpoints DELETE
- `DELETE /api/produtos/{id}` - Deletar produto

---

## Como Executar

### Opção 1: Visual Studio Code
```bash
# Terminal 1 - Executar apiPedidos
cd GatewayProject/Services/apiPedidos
dotnet run

# Terminal 2 - Executar apiProdutos
cd GatewayProject/Services/apiProdutos
dotnet run
```

### Opção 2: Visual Studio
- Abrir a solução
- Configurar múltiplos projetos para inicialização (apiPedidos e apiProdutos)
- Pressionar F5

## Testar os Endpoints

### Usando REST Client (VS Code Extension)
Abra os arquivos `.http` em cada projeto e clique em "Send Request" acima de cada endpoint:
- `apiPedidos/apiPedidos.http`
- `apiProdutos/apiProdutos.http`

### Usando cURL
```bash
# Obter todos os produtos
curl http://localhost:5004/api/produtos

# Obter um produto específico
curl http://localhost:5004/api/produtos/1

# Criar novo produto
curl -X POST http://localhost:5004/api/produtos \
  -H "Content-Type: application/json" \
  -d '{
    "nome":"Novo Produto",
    "descricao":"Descrição",
    "preco":100,
    "estoque":10,
    "categoria":"Categoria",
    "ativo":true
  }'

# Atualizar estoque (adiciona 5 unidades)
curl -X PUT http://localhost:5004/api/produtos/1/estoque/5

# Desativar produto
curl -X PUT http://localhost:5004/api/produtos/1/desativar
```

### Usando Postman
1. Importe os endpoints manualmente ou use a URL do Swagger/OpenAPI
2. OpenAPI URLs:
   - apiPedidos: `http://localhost:5026/openapi/v1.json`
   - apiProdutos: `http://localhost:5004/openapi/v1.json`

## Dados Mocados

### Produtos (apiProdutos)
| ID | Nome | Preço | Estoque | Categoria |
|----|----|-------|---------|-----------|
| 1 | Notebook | R$ 4.500,00 | 10 | Informática |
| 2 | Mouse | R$ 89,90 | 50 | Periféricos |
| 3 | Teclado | R$ 299,90 | 25 | Periféricos |
| 4 | Monitor | R$ 1.200,00 | 8 | Monitores |
| 5 | Webcam | R$ 199,90 | 15 | Periféricos |
| 6 | Fone de Ouvido | R$ 599,90 | 20 | Áudio |

### Pedidos (apiPedidos)
| ID | Cliente | Status | Valor | Data |
|----|---------|--------|-------|------|
| 1 | João Silva | Entregue | R$ 250,50 | -5 dias |
| 2 | Maria Santos | Processando | R$ 150,00 | -2 dias |
| 3 | Carlos Oliveira | Pendente | R$ 500,00 | Hoje |

## Detalhes Técnicos

- **Framework**: ASP.NET Core 10.0
- **Language**: C# 13
- **Dados**: Armazenados em memória (List<T>)
- **CORS**: Habilitado para todas as origens
- **Documentação**: OpenAPI/Swagger disponível em `/openapi/v1.json`
- **Validação**: Básica nos endpoints POST

## Exemplos de Requisições

### Criar Pedido
```json
POST /api/pedidos
{
  "clienteId": 104,
  "clienteNome": "Ana Costa",
  "valorTotal": 350.00,
  "status": "Pendente",
  "itens": [
    {
      "produtoId": 1,
      "produtoNome": "Notebook",
      "quantidade": 1,
      "precoUnitario": 350.00,
      "subtotal": 350.00
    }
  ]
}
```

### Criar Produto
```json
POST /api/produtos
{
  "nome": "Impressora",
  "descricao": "Impressora multifuncional 3 em 1",
  "preco": 599.90,
  "estoque": 12,
  "categoria": "Periféricos",
  "ativo": true
}
```

## Próximos Passos

Para migrar para um banco de dados real:
1. Adicionar Entity Framework Core
2. Substituir `PedidoService` e `ProdutoService` com implementações que usem DbContext
3. Adicionar migrations
4. Atualizar `Program.cs` para registrar o DbContext
5. Substituir List<T> com queries de banco de dados

## Notas

- Os dados são armazenados em memória e serão perdidos quando a aplicação é reiniciada
- Cada instância da aplicação tem sua própria cópia dos dados
- As operações de CRUD funcionam normalmente (Create, Read, Update, Delete)
- IDs são auto-gerados incrementando o ID máximo existente
- Os dados mocados incluem exemplos realistas de produtos e pedidos
