using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetNews.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FacebookUrl",
                table: "SystemSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Slogan",
                table: "SystemSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "YoutubeUrl",
                table: "SystemSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FacebookUrl",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "Slogan",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "YoutubeUrl",
                table: "SystemSettings");
        }
    }
}
