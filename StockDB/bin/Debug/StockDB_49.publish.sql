﻿/*
Deployment script for StockDB

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "StockDB"
:setvar DefaultFilePrefix "StockDB"
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'Creating Procedure [dbo].[spFinancialDataApi_GetQuarterly]...';


GO
CREATE PROCEDURE [dbo].[spFinancialDataApi_GetQuarterly]
	@stockId int
AS
begin
	set nocount on;

	SELECT 
		fa.Title,
		fa.Label,
		fac.DisplayName,
		ff.Frame,
		cv.Value AS CurrencyValue
	FROM
		dbo.StockFinances sf
		INNER JOIN dbo.Stocks s ON sf.StockId = s.Id
		INNER JOIN dbo.FinancialAttributes fa ON sf.FinancialAttributeId = fa.Id
		INNER JOIN dbo.FinancialFacts ff ON sf.FinancialFactId = ff.Id
		INNER JOIN dbo.Units u ON ff.UnitId = u.Id
		LEFT JOIN dbo.CurrencyValues cv ON sf.FinancialFactId = cv.FinancialFactId
		LEFT JOIN dbo.ShareValues sv ON sf.FinancialFactId = sv.FinancialFactId
		LEFT JOIN dbo.CurrencyPerShareValues cpsv ON sf.FinancialFactId = cpsv.FinancialFactId
		INNER JOIN dbo.FinancialAttributeConfig fac on fac.FinancialAttributeId = fa.Id
	WHERE 
		s.Id = @stockId
		AND u.Type = 'USD'
		AND Frame IS NOT NULL
	ORDER BY 
		fa.Title, FiscalYear DESC, Frame DESC;

end
GO
PRINT N'Update complete.';


GO
