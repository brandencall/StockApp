CREATE PROCEDURE [dbo].[spFinancialAttributes_Insert]
	@title nvarchar(500),
	@label nvarchar(1000),
	@description nvarchar(MAX)
AS
begin
	set nocount on;

	if not exists (select 1 from dbo.FinancialAttributes where Title = @title)
	begin
		insert into dbo.FinancialAttributes (Title, Label, Description)
		values (@title, @label, @description)
	end

	select top 1 [Id], [Title], [Label], [Description]
	from dbo.FinancialAttributes
	where Title = @title
end
