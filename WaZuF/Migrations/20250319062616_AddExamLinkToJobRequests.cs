using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaZuF.Migrations
{
    /// <inheritdoc />
    public partial class AddExamLinkToJobRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExamLink",
                table: "JobRequests",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamLink",
                table: "JobRequests");
        }
    }
}
