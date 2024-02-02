using Microsoft.Data.SqlClient;

namespace VisualAcademy.Infrastructures;

public class DefaultSchemaEnhancerAddColumns
{
    private string _defaultConnectionString;

    public DefaultSchemaEnhancerAddColumns(string defaultConnectionString)
    {
        _defaultConnectionString = defaultConnectionString;
    }

    public void EnhanceDefaultDatabase()
    {
        AddColumnIfNotExists("Tenants", "PortalName", "nvarchar(max) NULL DEFAULT ('VisualAcademy')");
        AddColumnIfNotExists("Tenants", "ScreeningPartnerName", "nvarchar(max) NULL DEFAULT ('VisualAcademy')");
    }

    private void AddColumnIfNotExists(string tableName, string columnName, string columnDefinition)
    {
        using (SqlConnection connection = new SqlConnection(_defaultConnectionString))
        {
            connection.Open();

            // Check if the column exists
            SqlCommand cmdCheck = new SqlCommand($@"
                    IF NOT EXISTS (
                        SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
                        WHERE TABLE_NAME = @tableName AND COLUMN_NAME = @columnName
                    ) 
                    BEGIN
                        ALTER TABLE dbo.{tableName} ADD {columnName} {columnDefinition};
                    END", connection);

            cmdCheck.Parameters.AddWithValue("@tableName", tableName);
            cmdCheck.Parameters.AddWithValue("@columnName", columnName);

            cmdCheck.ExecuteNonQuery();

            connection.Close();
        }
    }
}
