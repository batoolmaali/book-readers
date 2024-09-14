using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookReaders.Migrations
{
    /// <inheritdoc />
    public partial class QuoteTableUpdate4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookKidVM");

            migrationBuilder.DropTable(
                name: "KidsVideosViewModel");

            migrationBuilder.DropTable(
                name: "RecommendedBookVM");

            migrationBuilder.RenameColumn(
                name: "quote",
                table: "Quotes",
                newName: "QuoteText");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuoteText",
                table: "Quotes",
                newName: "quote");

            migrationBuilder.CreateTable(
                name: "BookKidVM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    KidsCategoryId = table.Column<int>(type: "int", nullable: false),
                    PDF = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookKidVM", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KidsVideosViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ThumbnailUrl = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KidsVideosViewModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecommendedBookVM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssociatedMood = table.Column<int>(type: "int", nullable: true),
                    AssociatedSeason = table.Column<int>(type: "int", nullable: true),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecommendedBookVM", x => x.Id);
                });
        }
    }
}
