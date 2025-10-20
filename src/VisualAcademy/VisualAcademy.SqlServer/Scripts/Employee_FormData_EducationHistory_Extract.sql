-- 특정 Employee의 학력(education_history) 펼치기
SELECT e.ID AS EmployeeID, ed.*
FROM dbo.Employees AS e
CROSS APPLY OPENJSON(e.FormData, '$.education_history')
WITH
(
    institution             NVARCHAR(255)   '$.institution',
    address                 NVARCHAR(MAX)   '$.address',
    city                    NVARCHAR(255)   '$.city',
    state                   NVARCHAR(50)    '$.state',
    country                 NVARCHAR(255)   '$.country',
    degree                  NVARCHAR(255)   '$.degree',
    major                   NVARCHAR(255)   '$.major',

    graduate                NVARCHAR(50)    '$.graduate',            -- ← 문자열로
    attending               NVARCHAR(50)    '$.attending',           -- ← 문자열로

    start_date              NVARCHAR(50)    '$.start_date',
    end_date                NVARCHAR(50)    '$.end_date',

    [type]                  NVARCHAR(50)    '$.type',                -- High School / College / Other
    training_programs       NVARCHAR(MAX)   '$.training_programs',
    phone                   NVARCHAR(50)    '$.phone',
    years_completed         NVARCHAR(50)    '$.years_completed',
    graduate_date           NVARCHAR(50)    '$.graduate_date',
    college_graduate_date   NVARCHAR(50)    '$.college_graduate_date',

    completed               NVARCHAR(50)    '$.completed',           -- ← 문자열로
    program_type            NVARCHAR(255)   '$.program_type',
    diploma_degree          NVARCHAR(255)   '$.diploma_degree',
    name_at_graduation      NVARCHAR(255)   '$.name_at_graduation'
) AS ed
WHERE e.ID = 170003;  -- 필요 시 변수/파라미터로 교체
