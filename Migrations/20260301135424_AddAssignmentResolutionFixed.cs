using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DOTNETCORE_DEV.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignmentResolutionFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssignmentResolutions",
                columns: table => new
                {
                    ResolutionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssignmentId = table.Column<int>(type: "int", nullable: false),
                    IncidentId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Component = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ResolutionDetails = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    EstimatedStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstimatedEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualCompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HoursSpent = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ResolutionStatus = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CompletionNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentResolutions", x => x.ResolutionId);
                    table.ForeignKey(
                        name: "FK_AssignmentResolutions_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_AssignmentResolutions_IncidentAssignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "IncidentAssignments",
                        principalColumn: "AssignmentId");
                    table.ForeignKey(
                        name: "FK_AssignmentResolutions_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "IncidentId");
                });

            migrationBuilder.CreateTable(
                name: "AssignmentResolutionAttachments",
                columns: table => new
                {
                    AttachmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResolutionId = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OriginalFileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UploadedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentResolutionAttachments", x => x.AttachmentId);
                    table.ForeignKey(
                        name: "FK_AssignmentResolutionAttachments_AssignmentResolutions_ResolutionId",
                        column: x => x.ResolutionId,
                        principalTable: "AssignmentResolutions",
                        principalColumn: "ResolutionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentResolutionAttachments_ResolutionId",
                table: "AssignmentResolutionAttachments",
                column: "ResolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentResolutionAttachments_UploadedAt",
                table: "AssignmentResolutionAttachments",
                column: "UploadedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentResolutions_AssignmentId",
                table: "AssignmentResolutions",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentResolutions_EmployeeId",
                table: "AssignmentResolutions",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentResolutions_EstimatedEndDate",
                table: "AssignmentResolutions",
                column: "EstimatedEndDate");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentResolutions_EstimatedStartDate",
                table: "AssignmentResolutions",
                column: "EstimatedStartDate");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentResolutions_IncidentId",
                table: "AssignmentResolutions",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentResolutions_ResolutionStatus",
                table: "AssignmentResolutions",
                column: "ResolutionStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignmentResolutionAttachments");

            migrationBuilder.DropTable(
                name: "AssignmentResolutions");
        }
    }
}
