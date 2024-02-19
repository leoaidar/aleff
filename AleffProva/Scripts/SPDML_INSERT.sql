CREATE OR ALTER PROCEDURE [dbo].[SPDML_INSERT]
    @TABLE_NAME        nvarchar(150),
    @INSERT_VALUES    nvarchar(max)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE 
    @INSERT_COLUMNS    nvarchar(max),
    @DML               nvarchar(max);

  SELECT @INSERT_COLUMNS = string_agg(name, ', ') WITHIN GROUP ( ORDER BY column_id )
  FROM sys.all_columns
  WHERE object_id = object_id(@TABLE_NAME) AND is_identity = 0;

  SELECT @DML = concat('INSERT INTO ',@TABLE_NAME,' (',@INSERT_COLUMNS,') VALUES (', @INSERT_VALUES, ')');

  EXEC SP_EXECUTESQL @DML;

  SELECT 1;

END
GO