using Microsoft.EntityFrameworkCore.Migrations;

namespace PremierKitchensDB.Migrations
{
    public partial class CustomerSetmandatoryfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_SourceOfInformation_SourceOfInformationID",
                table: "Customer");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Customer",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SourceOfInformationID",
                table: "Customer",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_SourceOfInformation_SourceOfInformationID",
                table: "Customer",
                column: "SourceOfInformationID",
                principalTable: "SourceOfInformation",
                principalColumn: "SourceOfInformationID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_SourceOfInformation_SourceOfInformationID",
                table: "Customer");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Customer",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<int>(
                name: "SourceOfInformationID",
                table: "Customer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_SourceOfInformation_SourceOfInformationID",
                table: "Customer",
                column: "SourceOfInformationID",
                principalTable: "SourceOfInformation",
                principalColumn: "SourceOfInformationID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
