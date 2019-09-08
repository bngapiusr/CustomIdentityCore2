using Microsoft.EntityFrameworkCore.Migrations;

namespace CustomIdentityCore2.Data.Migrations
{
    public partial class AddAccessProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Access",
                table: "Role",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Access",
                table: "Role");
        }
    }
}
