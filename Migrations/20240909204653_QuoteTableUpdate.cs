using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookReaders.Migrations
{
    /// <inheritdoc />
    public partial class QuoteTableUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Author",
                table: "Quotes",
                newName: "AuthorName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AuthorName",
                table: "Quotes",
                newName: "Author");
        }
    }
}
