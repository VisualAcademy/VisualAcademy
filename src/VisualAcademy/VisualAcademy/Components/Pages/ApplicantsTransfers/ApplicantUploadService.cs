using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace VisualAcademy.Components.Pages.ApplicantsTransfers;

public class ApplicantUploadService
{
    public async Task<List<ApplicantTransfer>> ParseExcelAsync(Stream fileStream)
    {
        var applicants = new List<ApplicantTransfer>();

        using var memoryStream = new MemoryStream();
        await fileStream.CopyToAsync(memoryStream);
        memoryStream.Position = 0; // Reset stream position

        using var spreadsheetDocument = SpreadsheetDocument.Open(memoryStream, false);

        // WorkbookPart null 가드
        WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart
            ?? throw new InvalidOperationException("WorkbookPart is missing in the Excel file.");

        // Sheet null 가드
        Sheet? sheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault()
            ?? throw new InvalidOperationException("No worksheet found in the Excel file.");

        // Id를 안전하게 string으로 추출
        string sheetId = sheet.Id?.Value
            ?? throw new InvalidOperationException("Worksheet Id is missing.");

        // WorksheetPart null 가드
        var worksheetPart = workbookPart.GetPartById(sheetId) as WorksheetPart
            ?? throw new InvalidOperationException("WorksheetPart not found for the given sheet Id.");

        // SheetData null 가드
        SheetData? sheetData = worksheetPart.Worksheet.Elements<SheetData>().FirstOrDefault()
            ?? throw new InvalidOperationException("SheetData not found in the worksheet.");

        foreach (Row row in sheetData.Elements<Row>().Skip(7))
        {
            var cellEnumerator = row.Elements<Cell>().GetEnumerator();

            cellEnumerator.MoveNext();
            var department = ReadCellValue(workbookPart, cellEnumerator.Current);

            cellEnumerator.MoveNext();
            var employeeId = ReadCellValue(workbookPart, cellEnumerator.Current);

            cellEnumerator.MoveNext();
            var firstName = ReadCellValue(workbookPart, cellEnumerator.Current);

            cellEnumerator.MoveNext();
            var middleInitial = ReadCellValue(workbookPart, cellEnumerator.Current);

            cellEnumerator.MoveNext();
            var lastName = ReadCellValue(workbookPart, cellEnumerator.Current);

            cellEnumerator.MoveNext();
            var dateBirthday = ReadCellValue(workbookPart, cellEnumerator.Current);

            cellEnumerator.MoveNext();
            var ssNumber = ReadCellValue(workbookPart, cellEnumerator.Current);

            cellEnumerator.MoveNext();
            var address1 = ReadCellValue(workbookPart, cellEnumerator.Current);

            cellEnumerator.MoveNext();
            var address2 = ReadCellValue(workbookPart, cellEnumerator.Current);

            cellEnumerator.MoveNext();
            var city = ReadCellValue(workbookPart, cellEnumerator.Current);

            cellEnumerator.MoveNext();
            var state = ReadCellValue(workbookPart, cellEnumerator.Current);

            cellEnumerator.MoveNext();
            var zip = ReadCellValue(workbookPart, cellEnumerator.Current);

            cellEnumerator.MoveNext();
            var gender = ReadCellValue(workbookPart, cellEnumerator.Current);

            cellEnumerator.MoveNext();
            var cellPhone = ReadCellValue(workbookPart, cellEnumerator.Current);

            cellEnumerator.MoveNext();
            var primaryEmail = ReadCellValue(workbookPart, cellEnumerator.Current);

            // 이후 열들에 대한 처리...
            // ...

            cellEnumerator.MoveNext();
            var employmentStatus = ReadCellValue(workbookPart, cellEnumerator.Current);

            cellEnumerator.MoveNext();
            var dateSeniority = ReadCellValue(workbookPart, cellEnumerator.Current);

            cellEnumerator.MoveNext();
            var defaultJobsHR = ReadCellValue(workbookPart, cellEnumerator.Current);

            cellEnumerator.MoveNext();
            var employeeStatus = ReadCellValue(workbookPart, cellEnumerator.Current);

            cellEnumerator.MoveNext();
            var tribalNation = ReadCellValue(workbookPart, cellEnumerator.Current);

            cellEnumerator.MoveNext();
            var gamingLicenseType = ReadCellValue(workbookPart, cellEnumerator.Current);

            cellEnumerator.MoveNext();
            var dateRehired = ReadCellValue(workbookPart, cellEnumerator.Current);

            cellEnumerator.MoveNext();
            var dateTerminated = ReadCellValue(workbookPart, cellEnumerator.Current);

            applicants.Add(new ApplicantTransfer
            {
                DepartmentName = department,
                EmployeeId = employeeId,
                FirstName = firstName,
                MiddleName = middleInitial,
                LastName = lastName,
                DOB = dateBirthday,
                SSN = ssNumber,
                Address = address1 + " " + address2,
                City = city,
                State = state,
                PostalCode = zip,
                Gender = gender,
                CellPhone = cellPhone,
                HomePhone = cellPhone,
                SecondaryPhone = cellPhone,
                WorkPhone = cellPhone,
                Email = primaryEmail,
                PrimaryEmail = primaryEmail,
                // 추가적으로 필요한 매핑을 계속 진행...
                EmploymentStatus = employmentStatus,
                DateSeniority = dateSeniority,
                DefaultJobsHR = defaultJobsHR,
                EmployeeStatus = employeeStatus,
                TribalNation = tribalNation,
                GamingLicenseType = gamingLicenseType,
                DateRehired = dateRehired,
                DateTerminated = dateTerminated,
            });
        }

        return applicants;
    }

    private static string ReadCellValue(WorkbookPart workbookPart, Cell? cell)
    {
        if (cell == null)
        {
            return string.Empty;
        }

        // SharedString인 경우
        if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
        {
            var sharedStringPart = workbookPart.SharedStringTablePart
                ?? throw new InvalidOperationException("SharedStringTablePart is missing.");

            var sharedStringTable = sharedStringPart.SharedStringTable
                ?? throw new InvalidOperationException("SharedStringTable is missing.");

            var rawIndex = cell.CellValue?.InnerText;
            if (string.IsNullOrEmpty(rawIndex) || !int.TryParse(rawIndex, out var index))
            {
                return string.Empty;
            }

            var sharedStringItem = sharedStringTable.Elements<SharedStringItem>().ElementAtOrDefault(index);
            return sharedStringItem?.InnerText ?? string.Empty;
        }

        // 일반 값
        return cell.CellValue?.InnerText ?? string.Empty;
    }
}
