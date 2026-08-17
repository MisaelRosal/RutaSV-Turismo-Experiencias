import { Link } from 'react-router-dom'

export default function CatalogPage() {
  return (
    <main className="max-w-7xl mx-auto px-4 py-8">
      <h1 className="text-3xl font-bold text-verde-bosque mb-4">Catálogo</h1>
      <p className="mb-6 text-cafe">Aquí irá el carrusel, la búsqueda, los filtros y las tarjetas de experiencias.</p>
      <Link
        to="/experiencias/1"
        className="inline-block px-4 py-2 rounded-md bg-terracota text-white font-semibold hover:bg-verde-bosque transition-colors"
      >
        Ver ejemplo de ficha (experiencia 1)
      </Link>
    </main>
  )
}