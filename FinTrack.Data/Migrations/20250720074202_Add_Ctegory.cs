using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTrack.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_Ctegory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinanceCategoryType",
                table: "Finances");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "Finances",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Finances_CategoryId",
                table: "Finances",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthTokens_UserId",
                table: "AuthTokens",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthTokens_Users_UserId",
                table: "AuthTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Finances_Categories_CategoryId",
                table: "Finances",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthTokens_Users_UserId",
                table: "AuthTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Finances_Categories_CategoryId",
                table: "Finances");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Finances_CategoryId",
                table: "Finances");

            migrationBuilder.DropIndex(
                name: "IX_AuthTokens_UserId",
                table: "AuthTokens");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Finances");

            migrationBuilder.AddColumn<int>(
                name: "FinanceCategoryType",
                table: "Finances",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
