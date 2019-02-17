using Microsoft.EntityFrameworkCore.Migrations;

namespace PremierKitchensDB.Migrations
{
    public partial class Addaddresstosp_GetCustomerList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GetCustomerList_SourceOfInformation_SourceOfInformationID",
                table: "GetCustomerList");

            migrationBuilder.AlterColumn<int>(
                name: "SourceOfInformationID",
                table: "GetCustomerList",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_GetCustomerList_SourceOfInformation_SourceOfInformationID",
                table: "GetCustomerList",
                column: "SourceOfInformationID",
                principalTable: "SourceOfInformation",
                principalColumn: "SourceOfInformationID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GetCustomerList_SourceOfInformation_SourceOfInformationID",
                table: "GetCustomerList");

            migrationBuilder.AlterColumn<int>(
                name: "SourceOfInformationID",
                table: "GetCustomerList",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GetCustomerList_SourceOfInformation_SourceOfInformationID",
                table: "GetCustomerList",
                column: "SourceOfInformationID",
                principalTable: "SourceOfInformation",
                principalColumn: "SourceOfInformationID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
