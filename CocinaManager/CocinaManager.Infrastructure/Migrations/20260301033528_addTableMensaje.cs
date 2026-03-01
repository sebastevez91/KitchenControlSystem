using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CocinaManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addTableMensaje : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mensajes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Remitente = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Destinatario = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Asunto = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Cuerpo = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Leido = table.Column<bool>(type: "bit", nullable: false),
                    FechaEnvio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MensajePadreId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EliminadoPorRemitente = table.Column<bool>(type: "bit", nullable: false),
                    EliminadoPorDestinatario = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mensajes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mensajes_Mensajes_MensajePadreId",
                        column: x => x.MensajePadreId,
                        principalTable: "Mensajes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mensajes_MensajePadreId",
                table: "Mensajes",
                column: "MensajePadreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mensajes");
        }
    }
}
