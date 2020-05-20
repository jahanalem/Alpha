using Microsoft.EntityFrameworkCore.Migrations;

namespace Alpha.DataAccess.Migrations
{
    public partial class add_HtmlMetaTags_to_Article : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescriptionHtmlMetaTag",
                table: "Article",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Article",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "KeywordsHtmlMetaTag",
                table: "Article",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleHtmlMetaTag",
                table: "Article",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionHtmlMetaTag",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "KeywordsHtmlMetaTag",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "TitleHtmlMetaTag",
                table: "Article");
        }
    }
}
