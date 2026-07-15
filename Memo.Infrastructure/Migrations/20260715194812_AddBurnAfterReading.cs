using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Memo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBurnAfterReading : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBurnAfterReading",
                table: "Notes",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBurnAfterReading",
                table: "Notes");
        }
    }
}
