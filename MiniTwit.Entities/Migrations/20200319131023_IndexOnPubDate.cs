using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniTwit.Entities.Migrations
{
    public partial class IndexOnPubDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Messages_PubDate",
                table: "Messages",
                column: "PubDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Messages_PubDate",
                table: "Messages");
        }
    }
}
