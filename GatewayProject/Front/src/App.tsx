import React from 'react'
import { BrowserRouter, Routes, Route, Link, useLocation } from 'react-router-dom'
import Products from './pages/Products'
import Orders from './pages/Orders'

export default function App() {
  return (
    <BrowserRouter>
      <div className="min-h-screen bg-gradient-to-br from-gray-50 to-gray-100">
        {/* Header */}
        <header className="bg-white shadow-sm border-b border-gray-200">
          <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-4">
            <div className="flex items-center justify-between">
              <div className="flex items-center gap-3">
                <div className="w-10 h-10 bg-gradient-to-br from-blue-600 to-blue-700 rounded-lg flex items-center justify-center">
                  <span className="text-white font-bold text-lg">μ</span>
                </div>
                <h1 className="text-2xl font-bold text-gray-900">MicroServices Demo</h1>
              </div>
              <nav className="flex gap-1 bg-gray-100 p-1 rounded-lg">
                <NavLink to="/produtos" label="Produtos" />
                <NavLink to="/pedidos" label="Pedidos" />
              </nav>
            </div>
          </div>
        </header>

        {/* Main Content */}
        <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
          <Routes>
            <Route path="/" element={<Products/>} />
            <Route path="/produtos" element={<Products/>} />
            <Route path="/pedidos" element={<Orders/>} />
          </Routes>
        </main>

        {/* Footer */}
        <footer className="bg-white border-t border-gray-200 mt-12">
          <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
            <p className="text-center text-sm text-gray-600">
              Built with React, TypeScript, Tailwind CSS & ASP.NET Core
            </p>
          </div>
        </footer>
      </div>
    </BrowserRouter>
  )
}

function NavLink({ to, label }: { to: string; label: string }) {
  const location = useLocation()
  const isActive = location.pathname === to
  
  return (
    <Link
      to={to}
      className={`px-4 py-2 rounded-md font-medium transition-colors duration-200 ${
        isActive
          ? 'bg-blue-600 text-white'
          : 'text-gray-700 hover:bg-white'
      }`}
    >
      {label}
    </Link>
  )
}
