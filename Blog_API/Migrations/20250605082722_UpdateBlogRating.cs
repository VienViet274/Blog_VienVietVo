using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog_API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBlogRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "BlogRatings",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_BlogRatings_UserID",
                table: "BlogRatings",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogRatings_AspNetUsers_UserID",
                table: "BlogRatings",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogRatings_AspNetUsers_UserID",
                table: "BlogRatings");

            migrationBuilder.DropIndex(
                name: "IX_BlogRatings_UserID",
                table: "BlogRatings");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "BlogRatings");
        }
    }
}
