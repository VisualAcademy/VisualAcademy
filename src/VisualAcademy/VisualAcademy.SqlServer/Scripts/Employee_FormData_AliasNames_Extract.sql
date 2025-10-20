-- 특정 Employee의 과거 이름(alias_names) 펼치기
SELECT e.ID AS EmployeeID, a.*
FROM dbo.Employees AS e
CROSS APPLY OPENJSON(e.FormData, '$.alias_names')
WITH
(
    last_name      NVARCHAR(255)   '$.last_name',
    first_name     NVARCHAR(255)   '$.first_name',
    middle_name    NVARCHAR(255)   '$.middle_name',
    from_date      NVARCHAR(50)    '$.from_date',
    to_date        NVARCHAR(50)    '$.to_date',
    dba            NVARCHAR(255)   '$.dba',       -- Doing Business As
    [type]         NVARCHAR(100)   '$.type'       -- alias 유형
) AS a
WHERE e.ID = 170003;  -- 필요 시 파라미터로 교체
