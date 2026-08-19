-- =============================================
-- SCRIPT COMPLETO PARA POSTGRESQL
-- BASADO EN EL DIAGRAMA: RutaSV.drawio.png
-- VERSION CORREGIDA
-- =============================================
-- Correcciones aplicadas:
--   1. CREATE EXTENSION btree_gist (requerido por los EXCLUDE USING gist)
--   2. tsrange con cast explicito '2000-01-01'::date + time (date + time = timestamp)
--   3. EXCLUDE en reservas para evitar doble reserva en fechas solapadas
--   4. CHECK (hora_fin > hora_inicio) en horarios
--   5. Idempotencia: se puede volver a ejecutar (DROP IF EXISTS + transaccion)
--   6. Indice extra en notificaciones(destinatario_email)
-- =============================================

BEGIN;

-- Elimina todo si el script ya se ejecuto antes (permite re-ejecucion limpia)
DROP TABLE IF EXISTS notificaciones CASCADE;
DROP TABLE IF EXISTS reserva_horario CASCADE;
DROP TABLE IF EXISTS reservas CASCADE;
DROP TABLE IF EXISTS experiencias CASCADE;
DROP TABLE IF EXISTS horarios CASCADE;
DROP TABLE IF EXISTS imagenes_publicacion CASCADE;
DROP TABLE IF EXISTS publicacion_amenidad CASCADE;
DROP TABLE IF EXISTS amenidades CASCADE;
DROP TABLE IF EXISTS publicaciones CASCADE;
DROP TABLE IF EXISTS anfitriones CASCADE;
DROP TABLE IF EXISTS categorias CASCADE;
DROP TABLE IF EXISTS municipios CASCADE;
DROP TABLE IF EXISTS departamentos CASCADE;

-- Extensión necesaria para EXCLUDE USING gist con columnas INTEGER
-- (agrega el operador '=' para int dentro de los índices GiST)
CREATE EXTENSION IF NOT EXISTS btree_gist;

-- 1. DEPARTAMENTOS
CREATE TABLE departamentos (
    id SERIAL PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL UNIQUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 2. MUNICIPIOS
CREATE TABLE municipios (
    id SERIAL PRIMARY KEY,
    departamento_id INTEGER NOT NULL REFERENCES departamentos(id) ON DELETE CASCADE,
    nombre VARCHAR(100) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(departamento_id, nombre)
);

-- 3. CATEGORIAS (Ej: Hotel, Hostal, Finca, Glamping)
CREATE TABLE categorias (
    id SERIAL PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL UNIQUE,
    descripcion TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 4. ANFITRIONES (Usuarios que ofrecen hospedaje)
CREATE TABLE anfitriones (
    id SERIAL PRIMARY KEY,
    municipio_id INTEGER NOT NULL REFERENCES municipios(id) ON DELETE RESTRICT,
    nombre VARCHAR(100) NOT NULL,
    email VARCHAR(150) NOT NULL UNIQUE,
    telefono VARCHAR(20),
    direccion TEXT,
    foto_url TEXT, 
    verificado BOOLEAN DEFAULT FALSE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 5. PUBLICACIONES (Propiedades en alquiler)
CREATE TABLE publicaciones (
    id SERIAL PRIMARY KEY,
    anfitrion_id INTEGER NOT NULL REFERENCES anfitriones(id) ON DELETE CASCADE,
    categoria_id INTEGER NOT NULL REFERENCES categorias(id) ON DELETE RESTRICT,
    titulo VARCHAR(200) NOT NULL,
    descripcion TEXT,
    precio_por_noche DECIMAL(10,2) NOT NULL CHECK (precio_por_noche >= 0),
    capacidad_maxima INTEGER NOT NULL CHECK (capacidad_maxima > 0),
    habitaciones INTEGER DEFAULT 1,
    camas INTEGER DEFAULT 1,
    banos INTEGER DEFAULT 1,
    direccion_exacta TEXT,
    latitud DECIMAL(10,8),
    longitud DECIMAL(11,8),
    estado VARCHAR(20) DEFAULT 'activo' CHECK (estado IN ('activo', 'inactivo', 'eliminado')),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 6. AMENIDADES (Ej: WiFi, Piscina, Parqueo)
CREATE TABLE amenidades (
    id SERIAL PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL UNIQUE,
    icono VARCHAR(50), -- Nombre del icono (FontAwesome, etc.)
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 7. PUBLICACION_AMENIDAD (Relación Muchos a Muchos entre Publicaciones y Amenidades)
CREATE TABLE publicacion_amenidad (
    publicacion_id INTEGER NOT NULL REFERENCES publicaciones(id) ON DELETE CASCADE,
    amenidad_id INTEGER NOT NULL REFERENCES amenidades(id) ON DELETE CASCADE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (publicacion_id, amenidad_id)
);

-- 8. IMAGENES_PUBLICACION
CREATE TABLE imagenes_publicacion (
    id SERIAL PRIMARY KEY,
    publicacion_id INTEGER NOT NULL REFERENCES publicaciones(id) ON DELETE CASCADE,
    url TEXT NOT NULL,
    es_principal BOOLEAN DEFAULT FALSE,
    orden INTEGER DEFAULT 0,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 9. HORARIOS (Disponibilidad por franjas horarias o días)
CREATE TABLE horarios (
    id SERIAL PRIMARY KEY,
    publicacion_id INTEGER NOT NULL REFERENCES publicaciones(id) ON DELETE CASCADE,
    dia_semana INTEGER NOT NULL CHECK (dia_semana BETWEEN 1 AND 7), -- 1=Lunes, 7=Domingo
    hora_inicio TIME NOT NULL,
    hora_fin TIME NOT NULL,
    disponible BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    -- La hora de fin debe ser mayor a la de inicio
    CHECK (hora_fin > hora_inicio),
    -- Evitar solapamiento de horarios para una misma publicación y día.
    -- tsrange espera timestamp: date + time devuelve timestamp.
    -- Se usa una fecha arbitraria (2000-01-01) solo para armar el rango.
    CONSTRAINT horarios_no_overlap EXCLUDE USING gist (
        publicacion_id WITH =,
        dia_semana WITH =,
        tsrange('2000-01-01'::date + hora_inicio, '2000-01-01'::date + hora_fin) WITH &&
    )
);

-- 10. RESERVAS
CREATE TABLE reservas (
    id SERIAL PRIMARY KEY,
    publicacion_id INTEGER NOT NULL REFERENCES publicaciones(id) ON DELETE CASCADE,
    nombre_huesped VARCHAR(100) NOT NULL,
    email_huesped VARCHAR(150) NOT NULL,
    telefono_huesped VARCHAR(20),
    fecha_inicio DATE NOT NULL,
    fecha_fin DATE NOT NULL,
    numero_huespedes INTEGER NOT NULL CHECK (numero_huespedes > 0),
    precio_total DECIMAL(10,2) NOT NULL CHECK (precio_total >= 0),
    estado VARCHAR(20) DEFAULT 'pendiente' CHECK (estado IN ('pendiente', 'confirmada', 'cancelada', 'finalizada')),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    -- Validar que fecha_fin sea mayor a fecha_inicio
    CHECK (fecha_fin > fecha_inicio),
    -- Evita doble reserva: una misma publicación no puede tener reservas
    -- con fechas solapadas. daterange(fecha_inicio, fecha_fin) usa el rango
    -- [inicio, fin): la fecha de salida no choca con la llegada siguiente.
    -- Nota: una reserva 'cancelada' también bloquea el rango; si molesta,
    -- se puede borrar la reserva o liberar las fechas desde la app.
    CONSTRAINT reservas_no_overlap EXCLUDE USING gist (
        publicacion_id WITH =,
        daterange(fecha_inicio, fecha_fin) WITH &&
    )
);

-- 11. RESERVA_HORARIO (Relación Muchos a Muchos entre Reservas y Horarios)
CREATE TABLE reserva_horario (
    reserva_id INTEGER NOT NULL REFERENCES reservas(id) ON DELETE CASCADE,
    horario_id INTEGER NOT NULL REFERENCES horarios(id) ON DELETE CASCADE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (reserva_id, horario_id)
);

-- 12. EXPERIENCIAS (Actividades o tours ofrecidos dentro de una publicación)
CREATE TABLE experiencias (
    id SERIAL PRIMARY KEY,
    publicacion_id INTEGER NOT NULL REFERENCES publicaciones(id) ON DELETE CASCADE,
    nombre VARCHAR(200) NOT NULL,
    descripcion TEXT,
    duracion_horas INTEGER,
    precio_adicional DECIMAL(10,2) DEFAULT 0 CHECK (precio_adicional >= 0),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 13. NOTIFICACIONES (Para anfitriones o huéspedes)
CREATE TABLE notificaciones (
    id SERIAL PRIMARY KEY,
    reserva_id INTEGER NOT NULL REFERENCES reservas(id) ON DELETE CASCADE,
    tipo VARCHAR(30) NOT NULL CHECK (tipo IN ('confirmacion', 'cancelacion', 'recordatorio', 'mensaje')),
    mensaje TEXT NOT NULL,
    leida BOOLEAN DEFAULT FALSE,
    destinatario_email VARCHAR(150) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- =============================================
-- ÍNDICES RECOMENDADOS PARA MEJORAR EL RENDIMIENTO
-- =============================================

CREATE INDEX idx_municipios_departamento ON municipios(departamento_id);
CREATE INDEX idx_anfitriones_municipio ON anfitriones(municipio_id);
CREATE INDEX idx_publicaciones_anfitrion ON publicaciones(anfitrion_id);
CREATE INDEX idx_publicaciones_categoria ON publicaciones(categoria_id);
CREATE INDEX idx_publicaciones_estado ON publicaciones(estado);
CREATE INDEX idx_imagenes_publicacion_publicacion ON imagenes_publicacion(publicacion_id);
CREATE INDEX idx_horarios_publicacion ON horarios(publicacion_id);
CREATE INDEX idx_reservas_publicacion ON reservas(publicacion_id);
CREATE INDEX idx_reservas_fechas ON reservas(fecha_inicio, fecha_fin);
CREATE INDEX idx_reservas_estado ON reservas(estado);
CREATE INDEX idx_reserva_horario_reserva ON reserva_horario(reserva_id);
CREATE INDEX idx_experiencias_publicacion ON experiencias(publicacion_id);
CREATE INDEX idx_notificaciones_reserva ON notificaciones(reserva_id);
CREATE INDEX idx_notificaciones_destinatario ON notificaciones(destinatario_email);

-- =============================================
-- FUNCIÓN PARA ACTUALIZAR 'updated_at' AUTOMÁTICAMENTE
-- =============================================

-- Crear una función que actualice el campo updated_at
CREATE OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW.updated_at = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Aplicar el trigger a todas las tablas que tienen updated_at
CREATE TRIGGER update_departamentos_updated_at
    BEFORE UPDATE ON departamentos
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_municipios_updated_at
    BEFORE UPDATE ON municipios
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_categorias_updated_at
    BEFORE UPDATE ON categorias
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_anfitriones_updated_at
    BEFORE UPDATE ON anfitriones
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_publicaciones_updated_at
    BEFORE UPDATE ON publicaciones
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_amenidades_updated_at
    BEFORE UPDATE ON amenidades
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_imagenes_publicacion_updated_at
    BEFORE UPDATE ON imagenes_publicacion
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_horarios_updated_at
    BEFORE UPDATE ON horarios
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_reservas_updated_at
    BEFORE UPDATE ON reservas
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_experiencias_updated_at
    BEFORE UPDATE ON experiencias
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_notificaciones_updated_at
    BEFORE UPDATE ON notificaciones
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

COMMIT;

-- =============================================
-- FIN DEL SCRIPT
-- =============================================