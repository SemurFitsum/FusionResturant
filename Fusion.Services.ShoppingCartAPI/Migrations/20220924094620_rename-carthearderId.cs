using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fusion.Services.ShoppingCartAPI.Migrations
{
    public partial class renamecarthearderId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetails_CartHeaders_CartHeaderId",
                table: "CartDetails");

            migrationBuilder.DropIndex(
                name: "IX_CartDetails_CartHeaderId",
                table: "CartDetails");

            migrationBuilder.RenameColumn(
                name: "CareHeaderId",
                table: "CartHeaders",
                newName: "CartHeaderId");

            migrationBuilder.AddColumn<int>(
                name: "CareHeaderId",
                table: "CartDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CartDetails_CareHeaderId",
                table: "CartDetails",
                column: "CareHeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetails_CartHeaders_CareHeaderId",
                table: "CartDetails",
                column: "CareHeaderId",
                principalTable: "CartHeaders",
                principalColumn: "CartHeaderId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetails_CartHeaders_CareHeaderId",
                table: "CartDetails");

            migrationBuilder.DropIndex(
                name: "IX_CartDetails_CareHeaderId",
                table: "CartDetails");

            migrationBuilder.DropColumn(
                name: "CareHeaderId",
                table: "CartDetails");

            migrationBuilder.RenameColumn(
                name: "CartHeaderId",
                table: "CartHeaders",
                newName: "CareHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_CartDetails_CartHeaderId",
                table: "CartDetails",
                column: "CartHeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetails_CartHeaders_CartHeaderId",
                table: "CartDetails",
                column: "CartHeaderId",
                principalTable: "CartHeaders",
                principalColumn: "CareHeaderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
