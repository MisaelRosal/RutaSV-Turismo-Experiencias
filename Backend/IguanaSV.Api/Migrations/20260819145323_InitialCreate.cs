using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IguanaSV.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:btree_gist", ",,");

            migrationBuilder.CreateTable(
                name: "amenidades",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    icono = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("amenidades_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "categorias",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("categorias_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "departamentos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("departamentos_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "municipios",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    departamento_id = table.Column<int>(type: "integer", nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("municipios_pkey", x => x.id);
                    table.ForeignKey(
                        name: "municipios_departamento_id_fkey",
                        column: x => x.departamento_id,
                        principalTable: "departamentos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "anfitriones",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    municipio_id = table.Column<int>(type: "integer", nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    direccion = table.Column<string>(type: "text", nullable: true),
                    verificado = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("anfitriones_pkey", x => x.id);
                    table.ForeignKey(
                        name: "anfitriones_municipio_id_fkey",
                        column: x => x.municipio_id,
                        principalTable: "municipios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "publicaciones",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    anfitrion_id = table.Column<int>(type: "integer", nullable: false),
                    categoria_id = table.Column<int>(type: "integer", nullable: false),
                    titulo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    descripcion = table.Column<string>(type: "text", nullable: true),
                    precio_por_noche = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    capacidad_maxima = table.Column<int>(type: "integer", nullable: false),
                    habitaciones = table.Column<int>(type: "integer", nullable: true, defaultValue: 1),
                    camas = table.Column<int>(type: "integer", nullable: true, defaultValue: 1),
                    banos = table.Column<int>(type: "integer", nullable: true, defaultValue: 1),
                    direccion_exacta = table.Column<string>(type: "text", nullable: true),
                    latitud = table.Column<decimal>(type: "numeric(10,8)", precision: 10, scale: 8, nullable: true),
                    longitud = table.Column<decimal>(type: "numeric(11,8)", precision: 11, scale: 8, nullable: true),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValueSql: "'activo'::character varying"),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("publicaciones_pkey", x => x.id);
                    table.ForeignKey(
                        name: "publicaciones_anfitrion_id_fkey",
                        column: x => x.anfitrion_id,
                        principalTable: "anfitriones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "publicaciones_categoria_id_fkey",
                        column: x => x.categoria_id,
                        principalTable: "categorias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "experiencias",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    publicacion_id = table.Column<int>(type: "integer", nullable: false),
                    nombre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    descripcion = table.Column<string>(type: "text", nullable: true),
                    duracion_horas = table.Column<int>(type: "integer", nullable: true),
                    precio_adicional = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true, defaultValue: 0m),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("experiencias_pkey", x => x.id);
                    table.ForeignKey(
                        name: "experiencias_publicacion_id_fkey",
                        column: x => x.publicacion_id,
                        principalTable: "publicaciones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "horarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    publicacion_id = table.Column<int>(type: "integer", nullable: false),
                    dia_semana = table.Column<int>(type: "integer", nullable: false),
                    hora_inicio = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    hora_fin = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    disponible = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("horarios_pkey", x => x.id);
                    table.ForeignKey(
                        name: "horarios_publicacion_id_fkey",
                        column: x => x.publicacion_id,
                        principalTable: "publicaciones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "imagenes_publicacion",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    publicacion_id = table.Column<int>(type: "integer", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false),
                    es_principal = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    orden = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("imagenes_publicacion_pkey", x => x.id);
                    table.ForeignKey(
                        name: "imagenes_publicacion_publicacion_id_fkey",
                        column: x => x.publicacion_id,
                        principalTable: "publicaciones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "publicacion_amenidad",
                columns: table => new
                {
                    publicacion_id = table.Column<int>(type: "integer", nullable: false),
                    amenidad_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("publicacion_amenidad_pkey", x => new { x.publicacion_id, x.amenidad_id });
                    table.ForeignKey(
                        name: "publicacion_amenidad_amenidad_id_fkey",
                        column: x => x.amenidad_id,
                        principalTable: "amenidades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "publicacion_amenidad_publicacion_id_fkey",
                        column: x => x.publicacion_id,
                        principalTable: "publicaciones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reservas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    publicacion_id = table.Column<int>(type: "integer", nullable: false),
                    nombre_huesped = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email_huesped = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    telefono_huesped = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    fecha_inicio = table.Column<DateOnly>(type: "date", nullable: false),
                    fecha_fin = table.Column<DateOnly>(type: "date", nullable: false),
                    numero_huespedes = table.Column<int>(type: "integer", nullable: false),
                    precio_total = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValueSql: "'pendiente'::character varying"),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("reservas_pkey", x => x.id);
                    table.ForeignKey(
                        name: "reservas_publicacion_id_fkey",
                        column: x => x.publicacion_id,
                        principalTable: "publicaciones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notificaciones",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    reserva_id = table.Column<int>(type: "integer", nullable: false),
                    tipo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    mensaje = table.Column<string>(type: "text", nullable: false),
                    leida = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    destinatario_email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("notificaciones_pkey", x => x.id);
                    table.ForeignKey(
                        name: "notificaciones_reserva_id_fkey",
                        column: x => x.reserva_id,
                        principalTable: "reservas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reserva_horario",
                columns: table => new
                {
                    reserva_id = table.Column<int>(type: "integer", nullable: false),
                    horario_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("reserva_horario_pkey", x => new { x.reserva_id, x.horario_id });
                    table.ForeignKey(
                        name: "reserva_horario_horario_id_fkey",
                        column: x => x.horario_id,
                        principalTable: "horarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "reserva_horario_reserva_id_fkey",
                        column: x => x.reserva_id,
                        principalTable: "reservas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "amenidades_nombre_key",
                table: "amenidades",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "anfitriones_email_key",
                table: "anfitriones",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_anfitriones_municipio",
                table: "anfitriones",
                column: "municipio_id");

            migrationBuilder.CreateIndex(
                name: "categorias_nombre_key",
                table: "categorias",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "departamentos_nombre_key",
                table: "departamentos",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_experiencias_publicacion",
                table: "experiencias",
                column: "publicacion_id");

            migrationBuilder.CreateIndex(
                name: "horarios_no_overlap",
                table: "horarios",
                columns: new[] { "publicacion_id", "dia_semana" })
                .Annotation("Npgsql:IndexMethod", "gist");

            migrationBuilder.CreateIndex(
                name: "idx_horarios_publicacion",
                table: "horarios",
                column: "publicacion_id");

            migrationBuilder.CreateIndex(
                name: "idx_imagenes_publicacion_publicacion",
                table: "imagenes_publicacion",
                column: "publicacion_id");

            migrationBuilder.CreateIndex(
                name: "idx_municipios_departamento",
                table: "municipios",
                column: "departamento_id");

            migrationBuilder.CreateIndex(
                name: "municipios_departamento_id_nombre_key",
                table: "municipios",
                columns: new[] { "departamento_id", "nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_notificaciones_destinatario",
                table: "notificaciones",
                column: "destinatario_email");

            migrationBuilder.CreateIndex(
                name: "idx_notificaciones_reserva",
                table: "notificaciones",
                column: "reserva_id");

            migrationBuilder.CreateIndex(
                name: "IX_publicacion_amenidad_amenidad_id",
                table: "publicacion_amenidad",
                column: "amenidad_id");

            migrationBuilder.CreateIndex(
                name: "idx_publicaciones_anfitrion",
                table: "publicaciones",
                column: "anfitrion_id");

            migrationBuilder.CreateIndex(
                name: "idx_publicaciones_categoria",
                table: "publicaciones",
                column: "categoria_id");

            migrationBuilder.CreateIndex(
                name: "idx_publicaciones_estado",
                table: "publicaciones",
                column: "estado");

            migrationBuilder.CreateIndex(
                name: "idx_reserva_horario_reserva",
                table: "reserva_horario",
                column: "reserva_id");

            migrationBuilder.CreateIndex(
                name: "IX_reserva_horario_horario_id",
                table: "reserva_horario",
                column: "horario_id");

            migrationBuilder.CreateIndex(
                name: "idx_reservas_estado",
                table: "reservas",
                column: "estado");

            migrationBuilder.CreateIndex(
                name: "idx_reservas_fechas",
                table: "reservas",
                columns: new[] { "fecha_inicio", "fecha_fin" });

            migrationBuilder.CreateIndex(
                name: "idx_reservas_publicacion",
                table: "reservas",
                column: "publicacion_id");

            migrationBuilder.CreateIndex(
                name: "reservas_no_overlap",
                table: "reservas",
                column: "publicacion_id")
                .Annotation("Npgsql:IndexMethod", "gist");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "experiencias");

            migrationBuilder.DropTable(
                name: "imagenes_publicacion");

            migrationBuilder.DropTable(
                name: "notificaciones");

            migrationBuilder.DropTable(
                name: "publicacion_amenidad");

            migrationBuilder.DropTable(
                name: "reserva_horario");

            migrationBuilder.DropTable(
                name: "amenidades");

            migrationBuilder.DropTable(
                name: "horarios");

            migrationBuilder.DropTable(
                name: "reservas");

            migrationBuilder.DropTable(
                name: "publicaciones");

            migrationBuilder.DropTable(
                name: "anfitriones");

            migrationBuilder.DropTable(
                name: "categorias");

            migrationBuilder.DropTable(
                name: "municipios");

            migrationBuilder.DropTable(
                name: "departamentos");
        }
    }
}
