using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class MIIAPI01GenerateModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Salary",
                table: "Employees",
                newName: "PayrollRef");

            migrationBuilder.CreateTable(
                name: "Payrolls",
                columns: table => new
                {
                    PayrollId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PayAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LastPayDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payrolls", x => x.PayrollId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PayrollRef",
                table: "Employees",
                column: "PayrollRef",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Payrolls_PayrollRef",
                table: "Employees",
                column: "PayrollRef",
                principalTable: "Payrolls",
                principalColumn: "PayrollId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Payrolls_PayrollRef",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "Payrolls");

            migrationBuilder.DropIndex(
                name: "IX_Employees_PayrollRef",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "PayrollRef",
                table: "Employees",
                newName: "Salary");
        }
    }
}
