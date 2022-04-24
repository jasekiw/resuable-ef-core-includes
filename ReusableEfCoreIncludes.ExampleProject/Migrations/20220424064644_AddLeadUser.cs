using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReusableEfCoreIncludes.ExampleProject.Migrations
{
    public partial class AddLeadUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LeadUserId",
                table: "Department",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Department_LeadUserId",
                table: "Department",
                column: "LeadUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Department_User_LeadUserId",
                table: "Department",
                column: "LeadUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Department_User_LeadUserId",
                table: "Department");

            migrationBuilder.DropIndex(
                name: "IX_Department_LeadUserId",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "LeadUserId",
                table: "Department");
        }
    }
}
