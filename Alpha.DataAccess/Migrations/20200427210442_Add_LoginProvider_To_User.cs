using Microsoft.EntityFrameworkCore.Migrations;

namespace Alpha.DataAccess.Migrations
{
    public partial class Add_LoginProvider_To_User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LoginProvider",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoginProvider",
                table: "User");
        }
    }
}
