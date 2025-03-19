using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaZuF.Migrations
{
    /// <inheritdoc />
    public partial class ss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_CodeQuizs_CodeQuizId",
                table: "Exams");

            migrationBuilder.DropTable(
                name: "CodeQuizs");

            migrationBuilder.DropIndex(
                name: "IX_Exams_CodeQuizId",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "CodeQuizId",
                table: "Exams");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Exams",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exams_AppUserId",
                table: "Exams",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_AspNetUsers_AppUserId",
                table: "Exams",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_AspNetUsers_AppUserId",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_Exams_AppUserId",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Exams");

            migrationBuilder.AddColumn<int>(
                name: "CodeQuizId",
                table: "Exams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CodeQuizs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeQuizs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeQuizs_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exams_CodeQuizId",
                table: "Exams",
                column: "CodeQuizId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeQuizs_AppUserId",
                table: "CodeQuizs",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_CodeQuizs_CodeQuizId",
                table: "Exams",
                column: "CodeQuizId",
                principalTable: "CodeQuizs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
