using Microsoft.EntityFrameworkCore.Migrations;

namespace FindJobs.DataAccess.Migrations
{
    public partial class addgoogleimage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicantGoogleImage",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicantGoogleImage",
                table: "Applicants");
        }
    }
}
