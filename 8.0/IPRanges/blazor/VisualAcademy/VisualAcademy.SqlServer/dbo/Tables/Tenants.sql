CREATE TABLE Tenants (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),  -- 자동 증가하는 기본 키
    Name NVARCHAR(100)                    -- 테넌트의 이름
);