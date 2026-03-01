using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DOTNETCORE_DEV.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmailToNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Incidents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Incidents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeID", "Name", "SupportLevel" },
                values: new object[,]
                {
                    { 1, "สมชาย ใจดี", 1 },
                    { 2, "มานี รักดี", 1 }
                });

            migrationBuilder.InsertData(
                table: "ServiceTypes",
                columns: new[] { "serviceTypeId", "serviceTypesName" },
                values: new object[,]
                {
                    { 1, "MS Office" },
                    { 2, "PC/Mobile Device/Notebook/Printer" },
                    { 3, "ขอติดตั้ง Software" },
                    { 4, "ขอบริการเกี่ยวกับบัญชีผู้ใช้งาน" },
                    { 5, "ระบบงานภายนอก" },
                    { 6, "ระบบงานภายใน" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_EmployeeId",
                table: "Incidents",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_serviceTypeId",
                table: "Incidents",
                column: "serviceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_Employees_EmployeeId",
                table: "Incidents",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_ServiceTypes_serviceTypeId",
                table: "Incidents",
                column: "serviceTypeId",
                principalTable: "ServiceTypes",
                principalColumn: "serviceTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_Employees_EmployeeId",
                table: "Incidents");

            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_ServiceTypes_serviceTypeId",
                table: "Incidents");

            migrationBuilder.DropIndex(
                name: "IX_Incidents_EmployeeId",
                table: "Incidents");

            migrationBuilder.DropIndex(
                name: "IX_Incidents_serviceTypeId",
                table: "Incidents");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "serviceTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "serviceTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "serviceTypeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "serviceTypeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "serviceTypeId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "serviceTypeId",
                keyValue: 6);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Incidents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Incidents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
