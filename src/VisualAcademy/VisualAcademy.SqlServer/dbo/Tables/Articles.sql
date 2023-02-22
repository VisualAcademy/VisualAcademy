--[1] Table: Artices(게시판) 테이블
CREATE TABLE [dbo].[Articles]
(
	[Id] Int Not Null Primary Key Identity(1, 1),	-- 일련번호
	[Title] NVarChar(255) Not Null,					-- 제목
	
	-- TODO: Columns Add Region
	[Content] NVarChar(Max) Null,					-- 내용 => TODO: Not Null



	[IsPinned] Bit Null Default(0),				-- 공지글로 올리기 


	-- AuditableBase.cs 참조
	[CreatedBy] NVarChar(255) Null,			-- 등록자(Creator)
	[Created] DateTime Default(GetDate()),	-- 생성일
	[ModifiedBy] NVarChar(255) Null,		-- 수정자(LastModifiedBy)
	[Modified] DateTime Null,				-- 수정일(LastModified)
)
Go
