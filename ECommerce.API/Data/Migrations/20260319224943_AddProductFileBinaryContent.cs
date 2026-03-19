using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProductFileBinaryContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "BinaryContent",
                table: "Files",
                type: "bytea",
                nullable: false,
                defaultValue: Array.Empty<byte>());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BinaryContent",
                table: "Files");
        }
    }
}
