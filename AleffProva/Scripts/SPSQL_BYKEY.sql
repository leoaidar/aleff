CREATE OR ALTER PROCEDURE [dbo].[SPSQL_BYKEY]
    @TABLE_NAME        nvarchar(150),
    @VALUE_KEY    nvarchar(max)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE 
    @PRIMARYKEY_COLUMN               nvarchar(150),
    @SQL               nvarchar(max);

  SELECT @PRIMARYKEY_COLUMN = string_agg(name, ', ') WITHIN GROUP ( ORDER BY column_id )
  FROM sys.all_columns
  WHERE object_id = object_id(@TABLE_NAME) AND is_identity = 1;

  SELECT @SQL = concat('SELECT * FROM ',@TABLE_NAME,' WHERE ',@PRIMARYKEY_COLUMN,' = ', @VALUE_KEY, '');

  EXEC SP_EXECUTESQL @SQL;

END
GO