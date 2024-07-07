using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crossword.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Crosswords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Published = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    FinishDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Elapsed = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                    Rows = table.Column<int>(type: "INTEGER", nullable: false),
                    Columns = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Crosswords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GridChars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    X = table.Column<int>(type: "INTEGER", nullable: false),
                    Y = table.Column<int>(type: "INTEGER", nullable: false),
                    C = table.Column<char>(type: "TEXT", nullable: false),
                    CrosswordId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GridChars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GridChars_Crosswords_CrosswordId",
                        column: x => x.CrosswordId,
                        principalTable: "Crosswords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    X = table.Column<int>(type: "INTEGER", nullable: false),
                    Y = table.Column<int>(type: "INTEGER", nullable: false),
                    I = table.Column<int>(type: "INTEGER", nullable: false),
                    Direction = table.Column<int>(type: "INTEGER", nullable: false),
                    Answer = table.Column<string>(type: "TEXT", nullable: false),
                    Clue = table.Column<string>(type: "TEXT", nullable: false),
                    CrosswordId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Words_Crosswords_CrosswordId",
                        column: x => x.CrosswordId,
                        principalTable: "Crosswords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GridChars_CrosswordId",
                table: "GridChars",
                column: "CrosswordId");

            migrationBuilder.CreateIndex(
                name: "IX_Words_CrosswordId",
                table: "Words",
                column: "CrosswordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GridChars");

            migrationBuilder.DropTable(
                name: "Words");

            migrationBuilder.DropTable(
                name: "Crosswords");
        }
    }
}
