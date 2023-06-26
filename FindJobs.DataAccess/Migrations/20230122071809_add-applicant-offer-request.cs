using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindJobs.DataAccess.Migrations
{
    public partial class addapplicantofferrequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicantOfferRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicantEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OfferId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantOfferRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicantOfferRequest_Applicants_ApplicantEmail",
                        column: x => x.ApplicantEmail,
                        principalTable: "Applicants",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicantOfferRequest_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OfferDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    OfferId = table.Column<int>(type: "int", nullable: false),
                    ApplicantOfferRequestId = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfferDocuments_ApplicantDocuments_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "ApplicantDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfferDocuments_ApplicantOfferRequest_ApplicantOfferRequestId",
                        column: x => x.ApplicantOfferRequestId,
                        principalTable: "ApplicantOfferRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OfferDocuments_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantOfferRequest_ApplicantEmail",
                table: "ApplicantOfferRequest",
                column: "ApplicantEmail");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantOfferRequest_OfferId",
                table: "ApplicantOfferRequest",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferDocuments_ApplicantOfferRequestId",
                table: "OfferDocuments",
                column: "ApplicantOfferRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferDocuments_DocumentId",
                table: "OfferDocuments",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferDocuments_OfferId",
                table: "OfferDocuments",
                column: "OfferId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfferDocuments");

            migrationBuilder.DropTable(
                name: "ApplicantOfferRequest");
        }
    }
}
