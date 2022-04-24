using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReusableEfCoreIncludes.ExampleProject.Migrations
{
    public partial class AddChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Department_User_LeadUserId",
                table: "Department");

            migrationBuilder.AlterColumn<int>(
                name: "LeadUserId",
                table: "Department",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Department_User_LeadUserId",
                table: "Department",
                column: "LeadUserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Department_User_LeadUserId",
                table: "Department");

            migrationBuilder.AlterColumn<int>(
                name: "LeadUserId",
                table: "Department",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Department_User_LeadUserId",
                table: "Department",
                column: "LeadUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
