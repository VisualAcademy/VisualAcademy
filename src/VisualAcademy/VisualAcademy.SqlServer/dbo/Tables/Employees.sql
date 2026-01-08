CREATE TABLE [dbo].[Employees]
(
    [Id] INT NOT NULL PRIMARY KEY,

    [FinalApprovalDate] DATETIMEOFFSET(7) NULL, -- 최종 승인 날짜
)
