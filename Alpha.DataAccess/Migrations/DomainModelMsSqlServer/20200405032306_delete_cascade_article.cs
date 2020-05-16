using Microsoft.EntityFrameworkCore.Migrations;

namespace Alpha.DataAccess.Migrations
{
    public partial class delete_cascade_article : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Article_ArticleId",
                table: "Comment");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Article_ArticleId",
                table: "Comment",
                column: "ArticleId",
                principalTable: "Article",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Article_ArticleId",
                table: "Comment");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Article_ArticleId",
                table: "Comment",
                column: "ArticleId",
                principalTable: "Article",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
