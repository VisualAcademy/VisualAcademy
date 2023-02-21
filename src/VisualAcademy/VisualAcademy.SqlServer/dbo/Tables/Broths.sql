CREATE TABLE [dbo].[Broths] (
    [Id]      INT           IDENTITY (1, 1) NOT NULL,
    [Name]    NVARCHAR (25) NOT NULL,
    [IsVegan] BIT           NOT NULL,
    CONSTRAINT [PK_Broths] PRIMARY KEY CLUSTERED ([Id] ASC)
);

