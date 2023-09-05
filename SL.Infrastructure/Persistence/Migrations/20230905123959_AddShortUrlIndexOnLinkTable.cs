using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SL.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddShortUrlIndexOnLinkTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Link_ShortUrl",
                table: "Link",
                column: "ShortUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Link_ShortUrl",
                table: "Link");
        }
    }
}
