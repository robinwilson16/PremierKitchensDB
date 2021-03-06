﻿using Microsoft.EntityFrameworkCore.Migrations;

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

            migrationBuilder.Sql(AlterScript);
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

            migrationBuilder.Sql(RollbackScript);
        }

        private const string AlterScript = @"
            ALTER PROCEDURE [dbo].[sp_GetCustomerList]
	            @SearchString NVARCHAR(MAX),
	            @SortString NVARCHAR(MAX)
            AS
	            DECLARE @sql NVARCHAR(MAX)
	
	            IF @SearchString = ''
		            SET @SearchString = '1=1';

	            IF @SortString = ''
		            SET @SortString = 'C.Surname ASC, C.Forename ASC, C.CustomerID';

	            SET @sql = N'
	            SELECT
		            C.CustomerID,
		            C.LegacyCustomerID,
		            C.Surname,
		            C.Forename,
		            C.Title,
		            C.Email,
		            C.MobilePhone,
		            C.WorkPhone,
                    Address = 
			            AD.Address1
			            + CASE WHEN AD.Address2 IS NOT NULL THEN '', '' + AD.Address2 ELSE '''' END
			            + CASE WHEN AD.Address3 IS NOT NULL THEN '', '' + AD.Address3 ELSE '''' END
			            + CASE WHEN AD.Address4 IS NOT NULL THEN '', '' + AD.Address4 ELSE '''' END,
		            PostCode = 
			            CASE
				            WHEN AD.PostCodeIn IS NOT NULL THEN AD.PostCodeOut + '' '' + AD.PostCodeIn
				            ELSE NULL
			            END,
		            C.OrderValue,
		            C.CanBeContacted,
		            C.HasOutstandingRemedialWork,
		            C.DateOfEnquiry,
		            C.ShowroomID,
		            S.ShowroomName,
		            CA.Areas,
		            C.SourceOfInformationID,
		            C.CreatedDate,
		            C.CreatedBy,
		            C.UpdatedDate,
		            C.UpdatedBy
	            FROM Customer C
	            LEFT JOIN Showroom S
		            ON S.ShowroomID = C.ShowroomID
                LEFT JOIN Address AD
		            ON AD.CustomerID = C.CustomerID
		            AND AD.IsPrimary = 1
	            LEFT JOIN (
		            SELECT
			            CA.CustomerID,
			            Areas = 
				            STUFF (
					            (
						            SELECT 
							            '', '' + A.AreaName
						            FROM Area A
						            INNER JOIN CustomerArea CA2
							            ON CA2.AreaID = A.AreaID
						            WHERE 
							            CA2.CustomerID = CA.CustomerID
						            ORDER BY 
							            '', '' + A.AreaName 
						            FOR XML PATH(''''),
						            TYPE 
					            ).value(''.'',''varchar(max)'') ,1,2, '''' 
				            )
		            FROM CustomerArea CA
		            GROUP BY
			            CA.CustomerID	
	            ) CA
		            ON CA.CustomerID = C.CustomerID
	            WHERE
		            ' + @SearchString + '
	            ORDER BY
		            ' + @SortString

	            EXEC sp_executesql @sql
            RETURN 0
        ";

        private const string RollbackScript = @"
            ALTER PROCEDURE [dbo].[sp_GetCustomerList]
	            @SearchString NVARCHAR(MAX),
	            @SortString NVARCHAR(MAX)
            AS
	            DECLARE @sql NVARCHAR(MAX)
	
	            IF @SearchString = ''
		            SET @SearchString = '1=1';

	            IF @SortString = ''
		            SET @SortString = 'C.Surname ASC, C.Forename ASC, C.CustomerID';

	            SET @sql = N'
	            SELECT
		            C.CustomerID,
		            C.LegacyCustomerID,
		            C.Surname,
		            C.Forename,
		            C.Title,
		            C.Email,
		            C.MobilePhone,
		            C.WorkPhone,
		            C.OrderValue,
		            C.CanBeContacted,
		            C.HasOutstandingRemedialWork,
		            C.DateOfEnquiry,
		            C.ShowroomID,
		            S.ShowroomName,
		            CA.Areas,
		            C.SourceOfInformationID,
		            C.CreatedDate,
		            C.CreatedBy,
		            C.UpdatedDate,
		            C.UpdatedBy
	            FROM Customer C
	            LEFT JOIN Showroom S
		            ON S.ShowroomID = C.ShowroomID
	            LEFT JOIN (
		            SELECT
			            CA.CustomerID,
			            Areas = 
				            STUFF (
					            (
						            SELECT 
							            '', '' + A.AreaName
						            FROM Area A
						            INNER JOIN CustomerArea CA2
							            ON CA2.AreaID = A.AreaID
						            WHERE 
							            CA2.CustomerID = CA.CustomerID
						            ORDER BY 
							            '', '' + A.AreaName 
						            FOR XML PATH(''''),
						            TYPE 
					            ).value(''.'',''varchar(max)'') ,1,2, '''' 
				            )
		            FROM CustomerArea CA
		            GROUP BY
			            CA.CustomerID	
	            ) CA
		            ON CA.CustomerID = C.CustomerID
	            WHERE
		            ' + @SearchString + '
	            ORDER BY
		            ' + @SortString

	            EXEC sp_executesql @sql
            RETURN 0
        ";
    }
}
