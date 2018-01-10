using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Emma.Blog.Data.Migrations
{
    public partial class AddTableComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "Users",
                nullable: false,
                oldClrType: typeof(long))
                .OldAnnotation("MySQL:AutoIncrement", true);

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    CommentId = table.Column<long>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Content = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    DeleteFlag = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.CommentId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "Users",
                nullable: false,
                oldClrType: typeof(long))
                .Annotation("MySQL:AutoIncrement", true);
        }
    }
}
