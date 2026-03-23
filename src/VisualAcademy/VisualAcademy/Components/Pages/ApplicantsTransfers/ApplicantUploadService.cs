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
        memoryStream.Position = 0;

        using var spreadsheetDocument = SpreadsheetDocument.Open(memoryStream, false);

        WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart
            ?? throw new InvalidOperationException("WorkbookPart is missing in the Excel file.");

        Workbook workbook = workbookPart.Workbook
            ?? throw new InvalidOperationException("Workbook is missing in the Excel file.");

        Sheet sheet = workbook.Descendants<Sheet>().FirstOrDefault()
            ?? throw new InvalidOperationException("No worksheet found in the Excel file.");

        string sheetId = sheet.Id?.Value
            ?? throw new InvalidOperationException("Worksheet Id is missing.");

        WorksheetPart worksheetPart = workbookPart.GetPartById(sheetId) as WorksheetPart
            ?? throw new InvalidOperationException("WorksheetPart not found for the given sheet Id.");

        Worksheet worksheet = worksheetPart.Worksheet
            ?? throw new InvalidOperationException("Worksheet is missing in the Excel file.");

        SheetData sheetData = worksheet.Elements<SheetData>().FirstOrDefault()
            ?? throw new InvalidOperationException("SheetData not found in the worksheet.");

        foreach (Row row in sheetData.Elements<Row>().Skip(7))
        {
            using var cellEnumerator = row.Elements<Cell>().GetEnumerator();

            var department = SafeReadNextCellValue(workbookPart, cellEnumerator);
            var employeeId = SafeReadNextCellValue(workbookPart, cellEnumerator);
            var firstName = SafeReadNextCellValue(workbookPart, cellEnumerator);
            var middleInitial = SafeReadNextCellValue(workbookPart, cellEnumerator);
            var lastName = SafeReadNextCellValue(workbookPart, cellEnumerator);
            var dateBirthday = SafeReadNextCellValue(workbookPart, cellEnumerator);
            var ssNumber = SafeReadNextCellValue(workbookPart, cellEnumerator);
            var address1 = SafeReadNextCellValue(workbookPart, cellEnumerator);
            var address2 = SafeReadNextCellValue(workbookPart, cellEnumerator);
            var city = SafeReadNextCellValue(workbookPart, cellEnumerator);
            var state = SafeReadNextCellValue(workbookPart, cellEnumerator);
            var zip = SafeReadNextCellValue(workbookPart, cellEnumerator);
            var gender = SafeReadNextCellValue(workbookPart, cellEnumerator);
            var cellPhone = SafeReadNextCellValue(workbookPart, cellEnumerator);
            var primaryEmail = SafeReadNextCellValue(workbookPart, cellEnumerator);

            // 이후 열들에 대한 처리...
            // ...

            var employmentStatus = SafeReadNextCellValue(workbookPart, cellEnumerator);
            var dateSeniority = SafeReadNextCellValue(workbookPart, cellEnumerator);
            var defaultJobsHR = SafeReadNextCellValue(workbookPart, cellEnumerator);
            var employeeStatus = SafeReadNextCellValue(workbookPart, cellEnumerator);
            var tribalNation = SafeReadNextCellValue(workbookPart, cellEnumerator);
            var gamingLicenseType = SafeReadNextCellValue(workbookPart, cellEnumerator);
            var dateRehired = SafeReadNextCellValue(workbookPart, cellEnumerator);
            var dateTerminated = SafeReadNextCellValue(workbookPart, cellEnumerator);

            applicants.Add(new ApplicantTransfer
            {
                DepartmentName = department,
                EmployeeId = employeeId,
                FirstName = firstName,
                MiddleName = middleInitial,
                LastName = lastName,
                DOB = dateBirthday,
                SSN = ssNumber,
                Address = $"{address1} {address2}".Trim(),
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

    private static string SafeReadNextCellValue(WorkbookPart workbookPart, IEnumerator<Cell> enumerator)
    {
        if (!enumerator.MoveNext())
        {
            return string.Empty;
        }

        return ReadCellValue(workbookPart, enumerator.Current);
    }

    private static string ReadCellValue(WorkbookPart workbookPart, Cell? cell)
    {
        if (cell == null)
        {
            return string.Empty;
        }

        if (cell.DataType is not null && cell.DataType.Value == CellValues.SharedString)
        {
            SharedStringTablePart sharedStringPart = workbookPart.SharedStringTablePart
                ?? throw new InvalidOperationException("SharedStringTablePart is missing.");

            SharedStringTable sharedStringTable = sharedStringPart.SharedStringTable
                ?? throw new InvalidOperationException("SharedStringTable is missing.");

            string? rawIndex = cell.CellValue?.InnerText;
            if (string.IsNullOrEmpty(rawIndex) || !int.TryParse(rawIndex, out int index))
            {
                return string.Empty;
            }

            SharedStringItem? sharedStringItem = sharedStringTable.Elements<SharedStringItem>().ElementAtOrDefault(index);
            return sharedStringItem?.InnerText ?? string.Empty;
        }

        return cell.CellValue?.InnerText ?? string.Empty;
    }
}