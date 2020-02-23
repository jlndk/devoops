using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniTwit.Entities.Migrations
{
    public partial class MoveToDatetime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE \"Messages\" ALTER COLUMN \"PubDate\" TYPE timestamp without time zone USING to_timestamp(\"PubDate\")"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                "PubDate",
                "Messages",
                "integer",
                nullable: false,
                oldClrType: typeof(DateTime));
        }
    }
}