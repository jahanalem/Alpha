using Microsoft.EntityFrameworkCore.Migrations;

namespace Alpha.DataAccess.Migrations
{
    public partial class Add_Property_To_Article_DescriptionAsPlainText : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescriptionAsPlainText",
                table: "Article",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionAsPlainText",
                table: "Article");
        }
    }
}
