CREATE PROCEDURE [dbo].[spUnits_GetByType]
	@unitTypeString nvarchar(20)

AS
begin
	set nocount on;

	select top 1 [Id], [Type], [FinancialTableId]
	from dbo.Units
	where Type = @unitTypeString
end
