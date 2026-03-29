using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PedidosECommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PedidoHistorico2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PedidoHistorico_Pedidos_PedidoId",
                table: "PedidoHistorico");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PedidoHistorico",
                table: "PedidoHistorico");

            migrationBuilder.RenameTable(
                name: "PedidoHistorico",
                newName: "PedidoHistoricos");

            migrationBuilder.RenameIndex(
                name: "IX_PedidoHistorico_PedidoId",
                table: "PedidoHistoricos",
                newName: "IX_PedidoHistoricos_PedidoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PedidoHistoricos",
                table: "PedidoHistoricos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoHistoricos_Pedidos_PedidoId",
                table: "PedidoHistoricos",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PedidoHistoricos_Pedidos_PedidoId",
                table: "PedidoHistoricos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PedidoHistoricos",
                table: "PedidoHistoricos");

            migrationBuilder.RenameTable(
                name: "PedidoHistoricos",
                newName: "PedidoHistorico");

            migrationBuilder.RenameIndex(
                name: "IX_PedidoHistoricos_PedidoId",
                table: "PedidoHistorico",
                newName: "IX_PedidoHistorico_PedidoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PedidoHistorico",
                table: "PedidoHistorico",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoHistorico_Pedidos_PedidoId",
                table: "PedidoHistorico",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
