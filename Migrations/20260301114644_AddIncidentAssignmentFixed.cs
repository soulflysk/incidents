using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DOTNETCORE_DEV.Migrations
{
    /// <inheritdoc />
    public partial class AddIncidentAssignmentFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IncidentAssignments",
                columns: table => new
                {
                    AssignmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IncidentId = table.Column<int>(type: "int", nullable: false),
                    AssignedByEmployeeId = table.Column<int>(type: "int", nullable: false),
                    AssignedToEmployeeId = table.Column<int>(type: "int", nullable: false),
                    AssignedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssignmentStatus = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AssignmentNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AcceptedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ParentAssignmentId = table.Column<int>(type: "int", nullable: true),
                    AssignmentType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentAssignments", x => x.AssignmentId);
                    table.ForeignKey(
                        name: "FK_IncidentAssignments_Employees_AssignedByEmployeeId",
                        column: x => x.AssignedByEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_IncidentAssignments_Employees_AssignedToEmployeeId",
                        column: x => x.AssignedToEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_IncidentAssignments_IncidentAssignments_ParentAssignmentId",
                        column: x => x.ParentAssignmentId,
                        principalTable: "IncidentAssignments",
                        principalColumn: "AssignmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncidentAssignments_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "IncidentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeID", "Name", "SupportLevel" },
                values: new object[,]
                {
                    { 3, "วิชัย รักงาน", 2 },
                    { 4, "สมศรี มุ่งมั่น", 2 },
                    { 5, "ประเสริฐ ตั้งใจ", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncidentAssignments_AssignedByEmployeeId",
                table: "IncidentAssignments",
                column: "AssignedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentAssignments_AssignedDateTime",
                table: "IncidentAssignments",
                column: "AssignedDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentAssignments_AssignedToEmployeeId",
                table: "IncidentAssignments",
                column: "AssignedToEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentAssignments_AssignmentStatus",
                table: "IncidentAssignments",
                column: "AssignmentStatus");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentAssignments_AssignmentType",
                table: "IncidentAssignments",
                column: "AssignmentType");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentAssignments_IncidentId",
                table: "IncidentAssignments",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentAssignments_ParentAssignmentId",
                table: "IncidentAssignments",
                column: "ParentAssignmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncidentAssignments");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeID",
                keyValue: 5);
        }
    }
}
