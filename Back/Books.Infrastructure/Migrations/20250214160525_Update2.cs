using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Books.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Token",
                table: "BlackListeds",
                newName: "RefreshToken");

            migrationBuilder.RenameIndex(
                name: "IX_BlackListeds_Token",
                table: "BlackListeds",
                newName: "IX_BlackListeds_RefreshToken");

            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "BlackListeds",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "BlackListeds");

            migrationBuilder.RenameColumn(
                name: "RefreshToken",
                table: "BlackListeds",
                newName: "Token");

            migrationBuilder.RenameIndex(
                name: "IX_BlackListeds_RefreshToken",
                table: "BlackListeds",
                newName: "IX_BlackListeds_Token");
        }
    }
}
