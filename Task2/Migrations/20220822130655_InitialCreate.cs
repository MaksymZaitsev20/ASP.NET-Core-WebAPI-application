using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task2.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Cover = table.Column<string>(type: "TEXT", nullable: true),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    Author = table.Column<string>(type: "TEXT", nullable: true),
                    Genre = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Books", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BookId = table.Column<int>(type: "INTEGER", nullable: false),
                    Score = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Ratings", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_Ratings_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    BookId = table.Column<int>(type: "INTEGER", nullable: false),
                    Reviewer = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Reviews", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_Reviews_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateIndex(
                name: "IX_Ratings_BookId",
                table: "Ratings",
                column: "BookId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Reviews_BookId",
                table: "Reviews",
                column: "BookId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "Ratings");

            _ = migrationBuilder.DropTable(
                name: "Reviews");

            _ = migrationBuilder.DropTable(
                name: "Books");
        }
    }
}
