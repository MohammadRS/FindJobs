using Microsoft.EntityFrameworkCore.Migrations;

namespace FindJobs.DataAccess.Migrations
{
    public partial class AddJobCategoryId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobPosition",
                table: "Offers");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "JobPosition",
                table: "Offers",
                type: "nvarchar(max)",
                nullable: true);

          
        }
    }
}
