using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class MIIAPI01RemoveAccountRolesPK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountRoles_Accounts_NIK",
                table: "AccountRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountRoles",
                table: "AccountRoles");

            migrationBuilder.DropIndex(
                name: "IX_AccountRoles_NIK",
                table: "AccountRoles");

            migrationBuilder.DropColumn(
                name: "AccountRolesId",
                table: "AccountRoles");

            migrationBuilder.AlterColumn<string>(
                name: "NIK",
                table: "AccountRoles",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountRoles",
                table: "AccountRoles",
                columns: new[] { "NIK", "RolesId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AccountRoles_Accounts_NIK",
                table: "AccountRoles",
                column: "NIK",
                principalTable: "Accounts",
                principalColumn: "NIK",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountRoles_Accounts_NIK",
                table: "AccountRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountRoles",
                table: "AccountRoles");

            migrationBuilder.AlterColumn<string>(
                name: "NIK",
                table: "AccountRoles",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "AccountRolesId",
                table: "AccountRoles",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountRoles",
                table: "AccountRoles",
                column: "AccountRolesId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountRoles_NIK",
                table: "AccountRoles",
                column: "NIK");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountRoles_Accounts_NIK",
                table: "AccountRoles",
                column: "NIK",
                principalTable: "Accounts",
                principalColumn: "NIK",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
