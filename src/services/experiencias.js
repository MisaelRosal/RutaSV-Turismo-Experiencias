import { experiencias, departamentos, categorias } from '../data/seed.js'

const delay = (ms = 200) => new Promise((resolve) => setTimeout(resolve, ms))

export async function getExperiencias({ search = '', categoria = '', zona = '', tipo = '', precioMax = '' } = {}) {
  await delay()
  let resultados = [...experiencias]

  if (search) {
    const termino = search.toLowerCase()
    resultados = resultados.filter((e) =>
      e.titulo.toLowerCase().includes(termino) ||
      e.descripcion.toLowerCase().includes(termino)
    )
  }
  if (categoria) resultados = resultados.filter((e) => e.categoria === categoria)
  if (zona) resultados = resultados.filter((e) => e.departamento === zona)
  if (tipo) resultados = resultados.filter((e) => e.tipo === tipo)
  if (precioMax) resultados = resultados.filter((e) => e.precio <= Number(precioMax))

  return resultados
}

export async function getExperienciaById(id) {
  await delay()
  return experiencias.find((e) => e.id === Number(id)) ?? null
}

export async function getExperienciasPopulares(limite = 5) {
  await delay()
  return experiencias.filter((e) => e.popular).slice(0, limite)
}

export async function getDepartamentos() {
  await delay()
  return departamentos
}

export async function getCategorias() {
  await delay()
  return categorias
}
