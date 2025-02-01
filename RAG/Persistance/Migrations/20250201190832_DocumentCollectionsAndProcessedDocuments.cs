using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class DocumentCollectionsAndProcessedDocuments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DocumentCollectionId",
                table: "Documents",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DocumentCollections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentCollections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcessedDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    DocumentCollectionId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessedDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessedDocuments_DocumentCollections_DocumentCollectionId",
                        column: x => x.DocumentCollectionId,
                        principalTable: "DocumentCollections",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DocumentCollectionId",
                table: "Documents",
                column: "DocumentCollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessedDocuments_DocumentCollectionId",
                table: "ProcessedDocuments",
                column: "DocumentCollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_DocumentCollections_DocumentCollectionId",
                table: "Documents",
                column: "DocumentCollectionId",
                principalTable: "DocumentCollections",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_DocumentCollections_DocumentCollectionId",
                table: "Documents");

            migrationBuilder.DropTable(
                name: "ProcessedDocuments");

            migrationBuilder.DropTable(
                name: "DocumentCollections");

            migrationBuilder.DropIndex(
                name: "IX_Documents_DocumentCollectionId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "DocumentCollectionId",
                table: "Documents");
        }
    }
}
