using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Books.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_BookStatuses_BookStatusId",
                table: "Books");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookStatuses",
                table: "BookStatuses");

            migrationBuilder.RenameTable(
                name: "BookStatuses",
                newName: "BookStatus");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookStatus",
                table: "BookStatus",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BookStatus_BookStatusId",
                table: "Books",
                column: "BookStatusId",
                principalTable: "BookStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_BookStatus_BookStatusId",
                table: "Books");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookStatus",
                table: "BookStatus");

            migrationBuilder.RenameTable(
                name: "BookStatus",
                newName: "BookStatuses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookStatuses",
                table: "BookStatuses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BookStatuses_BookStatusId",
                table: "Books",
                column: "BookStatusId",
                principalTable: "BookStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
