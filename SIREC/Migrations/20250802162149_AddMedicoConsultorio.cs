using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIREC.Migrations
{
    /// <inheritdoc />
    public partial class AddMedicoConsultorio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MedicoConsultorios",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDUsuario = table.Column<int>(type: "int", nullable: false),
                    IDConsultorio = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicoConsultorios", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MedicoConsultorios_Consultorios_IDConsultorio",
                        column: x => x.IDConsultorio,
                        principalTable: "Consultorios",
                        principalColumn: "IDConsultorio",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicoConsultorios_Usuarios_IDUsuario",
                        column: x => x.IDUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IDUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicoConsultorios_IDConsultorio",
                table: "MedicoConsultorios",
                column: "IDConsultorio");

            migrationBuilder.CreateIndex(
                name: "IX_MedicoConsultorios_IDUsuario",
                table: "MedicoConsultorios",
                column: "IDUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicoConsultorios");
        }
    }
}
