CREATE PROCEDURE [dbo].[spStocks_GetAllCIKs]
	
AS
begin
	set nocount on;
	
	select CIK from dbo.Stocks
end
