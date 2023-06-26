using Microsoft.EntityFrameworkCore.Migrations;

namespace FindJobs.DataAccess.Migrations
{
    public partial class RemoveJobCategoryFromOffer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_JobCategories_JobCategoryId",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Offers_JobCategoryId",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "JobCategoryId",
                table: "Offers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobCategoryId",
                table: "Offers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offers_JobCategoryId",
                table: "Offers",
                column: "JobCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_JobCategories_JobCategoryId",
                table: "Offers",
                column: "JobCategoryId",
                principalTable: "JobCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
