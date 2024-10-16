CREATE TABLE [dbo].[TextTemplates]
(
    [ID] BIGINT NOT NULL PRIMARY KEY IDENTITY, -- 템플릿 아이디
    [Message] NVARCHAR(MAX) NOT NULL, -- 템플릿 메시지
    [Title] NVARCHAR(100) NULL -- 템플릿 제목
)
