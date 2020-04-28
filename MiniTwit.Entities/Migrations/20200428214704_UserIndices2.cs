using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniTwit.Entities.Migrations
{
    public partial class UserIndices2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Messages_AuthorId",
                table: "Messages");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_AuthorId_PubDate",
                table: "Messages",
                columns: new[] { "AuthorId", "PubDate" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Messages_AuthorId_PubDate",
                table: "Messages");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_AuthorId",
                table: "Messages",
                column: "AuthorId");
        }
    }
}
