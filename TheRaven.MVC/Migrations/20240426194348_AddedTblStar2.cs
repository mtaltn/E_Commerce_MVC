using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheRaven.MVC.Migrations
{
    /// <inheritdoc />
    public partial class AddedTblStar2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "RateAvg",
                table: "Products",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RateAvg",
                table: "Products");
        }
    }
}
