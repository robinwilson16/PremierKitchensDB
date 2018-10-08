USE [PremierKitchensDB]
GO

/****** Object: SqlProcedure [dbo].[sp_GetCustomerList] Script Date: 19/09/2018 00:38:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE sp_GetCustomerList
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
		C.TitleID,
		C.Email,
		C.MobilePhone,
		C.WorkPhone,
		C.IsExistingCustomer,
		C.OrderValue,
		C.CanBeContacted,
		C.HasOutstandingRemedialWork,
		C.DateOfEnquiry,
		C.ShowroomID,
		C.SourceOfInformationID,
		C.CreatedDate,
		C.CreatedBy,
		C.UpdatedDate,
		C.UpdatedBy
	FROM Customer C
	LEFT JOIN Showroom S
		ON S.ShowroomID = C.ShowroomID
	WHERE
		' + @SearchString + '
	ORDER BY
		' + @SortString

	EXEC sp_executesql @sql
RETURN 0
