CREATE TABLE [dbo].[LogAcesso]
(
	[LogAcessoId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UsuarioId] INT NULL, 
    [DataHoraAcesso] DATETIME NULL, 
    [EnderecoIp] VARCHAR(50) NULL
)
