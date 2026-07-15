using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Memo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddViewCount2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "Notes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "Notes");
        }
    }
}
