using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Api.Cart.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Session",
                columns: table => new
                {
                    CartSessionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Session", x => x.CartSessionId);
                });

            migrationBuilder.CreateTable(
                name: "SessionDetail",
                columns: table => new
                {
                    CartSessionDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SelectedProduct = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CartSessionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionDetail", x => x.CartSessionDetailId);
                    table.ForeignKey(
                        name: "FK_SessionDetail_Session_CartSessionId",
                        column: x => x.CartSessionId,
                        principalTable: "Session",
                        principalColumn: "CartSessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SessionDetail_CartSessionId",
                table: "SessionDetail",
                column: "CartSessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessionDetail");

            migrationBuilder.DropTable(
                name: "Session");
        }
    }
}
