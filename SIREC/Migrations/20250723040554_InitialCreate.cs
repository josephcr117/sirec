using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIREC.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Consultorios",
                columns: table => new
                {
                    IDConsultorio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Especialidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HorarioAtencion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultorios", x => x.IDConsultorio);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IDRol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.IDRol);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IDUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contraseña = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cedula = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IDUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Citas",
                columns: table => new
                {
                    IDCita = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hora = table.Column<TimeSpan>(type: "time", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IDUsuario = table.Column<int>(type: "int", nullable: false),
                    IDConsultorio = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citas", x => x.IDCita);
                    table.ForeignKey(
                        name: "FK_Citas_Consultorios_IDConsultorio",
                        column: x => x.IDConsultorio,
                        principalTable: "Consultorios",
                        principalColumn: "IDConsultorio",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Citas_Usuarios_IDUsuario",
                        column: x => x.IDUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IDUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioRoles",
                columns: table => new
                {
                    IDUsuario = table.Column<int>(type: "int", nullable: false),
                    IDRol = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioRoles", x => new { x.IDUsuario, x.IDRol });
                    table.ForeignKey(
                        name: "FK_UsuarioRoles_Roles_IDRol",
                        column: x => x.IDRol,
                        principalTable: "Roles",
                        principalColumn: "IDRol",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioRoles_Usuarios_IDUsuario",
                        column: x => x.IDUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IDUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Citas_IDConsultorio",
                table: "Citas",
                column: "IDConsultorio");

            migrationBuilder.CreateIndex(
                name: "IX_Citas_IDUsuario",
                table: "Citas",
                column: "IDUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioRoles_IDRol",
                table: "UsuarioRoles",
                column: "IDRol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Citas");

            migrationBuilder.DropTable(
                name: "UsuarioRoles");

            migrationBuilder.DropTable(
                name: "Consultorios");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
