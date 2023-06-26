using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindJobs.DataAccess.Migrations
{
    public partial class AddAppliacantOfferFavourite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicantOffersFavourites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicantEmail = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    OfferId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantOffersFavourites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicantOffersFavourites_Applicants_ApplicantEmail",
                        column: x => x.ApplicantEmail,
                        principalTable: "Applicants",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicantOffersFavourites_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantOffersFavourites_ApplicantEmail",
                table: "ApplicantOffersFavourites",
                column: "ApplicantEmail");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantOffersFavourites_OfferId",
                table: "ApplicantOffersFavourites",
                column: "OfferId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicantOffersFavourites");
        }
    }
}
