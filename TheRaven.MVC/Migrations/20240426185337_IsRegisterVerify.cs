using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheRaven.MVC.Migrations
{
    /// <inheritdoc />
    public partial class IsRegisterVerify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRegisterVerification",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRegisterVerification",
                table: "AspNetUsers");
        }
    }
}
