import React, { useEffect, useState } from 'react'
import { Card, CardContent, CardHeader, CardFooter } from '../components/Card'
import { Badge } from '../components/Badge'
import { Loading } from '../components/Loading'
import { Button } from '../components/Button'

type Item = { id:number, produtoNome:string, quantidade:number, subtotal:number }
type Pedido = { id:number, clienteNome:string, dataPedido:string, valorTotal:number, status:string, itens: Item[] }

const getStatusVariant = (status: string): 'success' | 'warning' | 'danger' | 'info' => {
  switch (status.toLowerCase()) {
    case 'entregue':
      return 'success'
    case 'processando':
      return 'info'
    case 'pendente':
      return 'warning'
    case 'cancelado':
      return 'danger'
    default:
      return 'info'
  }
}

export default function Orders(){
  const [pedidos, setPedidos] = useState<Pedido[]>([])
  const [loading, setLoading] = useState(true)

  useEffect(()=>{
    fetch('http://localhost:5170/api/pedidos')
      .then(r=>r.json())
      .then(data => {
        const items = data?.value ?? data ?? []
        setPedidos(items)
      })
      .catch(()=> setPedidos([]))
      .finally(()=> setLoading(false))
  }, [])

  return (
    <section>
      <div className="flex justify-between items-center mb-6">
        <h2 className="text-3xl font-bold text-gray-900">Pedidos</h2>
        <Badge variant="info">{pedidos.length} pedidos</Badge>
      </div>

      {loading ? (
        <Loading message="Carregando pedidos..." />
      ) : pedidos.length === 0 ? (
        <Card>
          <CardContent className="py-12 text-center">
            <p className="text-gray-500 mb-4">Nenhum pedido encontrado</p>
            <Button variant="primary">Novo Pedido</Button>
          </CardContent>
        </Card>
      ) : (
        <div className="space-y-4">
          {pedidos.map(p => (
            <Card key={p.id} className="hover:shadow-lg transition-shadow">
              <CardHeader>
                <div className="flex justify-between items-start gap-4">
                  <div className="flex-1">
                    <h3 className="font-bold text-lg text-gray-900">Pedido #{p.id}</h3>
                    <p className="text-sm text-gray-600 mt-1">Cliente: {p.clienteNome}</p>
                  </div>
                  <Badge variant={getStatusVariant(p.status)}>
                    {p.status}
                  </Badge>
                </div>
                <p className="text-xs text-gray-500 mt-2">
                  {new Date(p.dataPedido).toLocaleString('pt-BR')}
                </p>
              </CardHeader>

              <CardContent>
                <div className="bg-gray-50 rounded-lg p-4 mb-4">
                  <p className="text-sm font-semibold text-gray-700 mb-3">Itens do Pedido</p>
                  <div className="space-y-2">
                    {p.itens.map(i => (
                      <div key={i.id} className="flex justify-between items-center py-2 border-b border-gray-200 last:border-b-0">
                        <div className="flex-1">
                          <p className="text-sm font-medium text-gray-900">{i.produtoNome}</p>
                          <p className="text-xs text-gray-600">Qty: {i.quantidade}</p>
                        </div>
                        <p className="font-semibold text-gray-900">R$ {i.subtotal.toFixed(2)}</p>
                      </div>
                    ))}
                  </div>
                </div>

                <div className="flex justify-between items-center p-4 bg-blue-50 rounded-lg border border-blue-100">
                  <p className="font-semibold text-gray-900">Total do Pedido:</p>
                  <p className="text-2xl font-bold text-blue-600">R$ {p.valorTotal.toFixed(2)}</p>
                </div>
              </CardContent>

              <CardFooter className="flex gap-2">
                <Button variant="primary" size="sm" className="flex-1">
                  Ver Detalhes
                </Button>
                <Button variant="secondary" size="sm" className="flex-1">
                  Rastrear
                </Button>
              </CardFooter>
            </Card>
          ))}
        </div>
      )}
    </section>
  )
}
