using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crossword.Migrations
{
    /// <inheritdoc />
    public partial class AddGridCharStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "GridChars",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "GridChars");
        }
    }
}
