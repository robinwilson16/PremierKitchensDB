using Microsoft.EntityFrameworkCore.Migrations;

namespace PremierKitchensDB.Migrations
{
    public partial class Fixtosp_GetCustomerListnottoaddcommasforemptybutnonnullcolumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(AlterScript);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
		            Address = AD.Address,
		            PostCode = AD.PostCode,
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
			            AD.CustomerID,
			            Address = 
				            AD.Address1
				            + CASE WHEN COALESCE ( AD.Address2, '''' ) <> '''' THEN '', '' + AD.Address2 ELSE '''' END
							+ CASE WHEN COALESCE ( AD.Address3, '''' ) <> '''' THEN '', '' + AD.Address3 ELSE '''' END
							+ CASE WHEN COALESCE ( AD.Address4, '''' ) <> '''' THEN '', '' + AD.Address4 ELSE '''' END,
			            PostCode = 
				            CASE
					            WHEN AD.PostCodeIn IS NOT NULL THEN AD.PostCodeOut + '' '' + AD.PostCodeIn
					            ELSE NULL
				            END
		            FROM Address AD
		            WHERE
			            AD.IsPrimary = 1
	            ) AD
		            ON AD.CustomerID = C.CustomerID
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
		            Address = AD.Address,
		            PostCode = AD.PostCode,
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
			            AD.CustomerID,
			            Address = 
				            AD.Address1
				            + CASE WHEN AD.Address2 IS NOT NULL THEN '', '' + AD.Address2 ELSE '''' END
				            + CASE WHEN AD.Address3 IS NOT NULL THEN '', '' + AD.Address3 ELSE '''' END
				            + CASE WHEN AD.Address4 IS NOT NULL THEN '', '' + AD.Address4 ELSE '''' END,
			            PostCode = 
				            CASE
					            WHEN AD.PostCodeIn IS NOT NULL THEN AD.PostCodeOut + '' '' + AD.PostCodeIn
					            ELSE NULL
				            END
		            FROM Address AD
		            WHERE
			            AD.IsPrimary = 1
	            ) AD
		            ON AD.CustomerID = C.CustomerID
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
