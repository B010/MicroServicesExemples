import React, { useEffect, useState } from 'react'
import { Card, CardContent, CardHeader } from '../components/Card'
import { Badge } from '../components/Badge'
import { Loading } from '../components/Loading'
import { Button } from '../components/Button'

type Produto = {
    id: number
    nome: string
    descricao: string
    preco: number
    estoque: number
    categoria: string
    ativo: boolean
}

export default function Products() {
    const [produtos, setProdutos] = useState<Produto[]>([])
    const [loading, setLoading] = useState(true)

    useEffect(() => {
        fetch('http://localhost:5170/api/produtos')
            .then(r => r.json())
            .then(data => {
                // API returns { value: [...], Count }
                const items = data?.value ?? data ?? []
                setProdutos(items)
            })
            .catch(() => setProdutos([]))
            .finally(() => setLoading(false))
    }, [])

    return (
        <section>
            <div className="flex justify-between items-center mb-6">
                <h2 className="text-3xl font-bold text-gray-900">Produtos</h2>
                <Badge variant="info">{produtos.length} produtos</Badge>
            </div>

            {loading ? (
                <Loading message="Carregando produtos..." />
            ) : produtos.length === 0 ? (
                <Card>
                    <CardContent className="py-12 text-center">
                        <p className="text-gray-500 mb-4">Nenhum produto encontrado</p>
                        <Button variant="primary">Adicionar Produto</Button>
                    </CardContent>
                </Card>
            ) : (
                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                    {produtos.map(p => (
                        <Card key={p.id} className="hover:shadow-lg transition-shadow">
                            <CardHeader>
                                <div className="flex justify-between items-start gap-2">
                                    <div className="flex-1">
                                        <h3 className="font-bold text-lg text-gray-900">{p.nome}</h3>
                                        <p className="text-sm text-gray-600 mt-1">{p.categoria}</p>
                                    </div>
                                    <Badge variant={p.ativo ? 'success' : 'danger'}>
                                        {p.ativo ? 'Ativo' : 'Inativo'}
                                    </Badge>
                                </div>
                            </CardHeader>

                            <CardContent>
                                <p className="text-sm text-gray-700 mb-4">{p.descricao}</p>

                                <div className="flex justify-between items-center mb-4">
                                    <div>
                                        <p className="text-xs text-gray-500">Preço</p>
                                        <p className="text-2xl font-bold text-blue-600">R$ {p.preco.toFixed(2)}</p>
                                    </div>
                                    <div>
                                        <p className="text-xs text-gray-500">Estoque</p>
                                        <p className="text-2xl font-bold text-gray-900">{p.estoque}</p>
                                    </div>
                                </div>

                                {p.estoque === 0 && (
                                    <Badge variant="danger" className="w-full justify-center text-sm py-1 mb-4">
                                        Fora de Estoque
                                    </Badge>
                                )}
                            </CardContent>

                            <div className="px-6 py-4 border-t border-gray-200 bg-gray-50 flex gap-2">
                                <Button variant="primary" size="sm" className="flex-1">
                                    Ver Detalhes
                                </Button>
                                <Button variant="secondary" size="sm" className="flex-1">
                                    Adicionar
                                </Button>
                            </div>
                        </Card>
                    ))}
                </div>
            )}
        </section>
    )
}
