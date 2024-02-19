CREATE TABLE [dbo].[Table]
(
	[UsuarioId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Nome] VARCHAR(100) NULL, 
    [Login] VARCHAR(50) NULL, 
    [Senha] VARCHAR(MAX) NULL, 
    [IsAdmin] BIT NULL
)