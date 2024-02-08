using Microsoft.Data.SqlClient;

namespace VisualAcademy.Infrastructures;

public class DefaultSchemaEnhancerCreateAllowedIPRangesTable(string defaultConnectionString)
{
    public void EnhanceDefaultDatabase()
    {
        CreateAllowedIPRangesTableIfNotExists();
    }

    private void CreateAllowedIPRangesTableIfNotExists()
    {
        using (SqlConnection connection = new SqlConnection(defaultConnectionString))
        {
            connection.Open();

            SqlCommand cmdCheck = new SqlCommand(@"
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_SCHEMA = 'dbo' 
                    AND TABLE_NAME = 'AllowedIPRanges'", connection);

            int tableCount = (int)cmdCheck.ExecuteScalar();

            if (tableCount == 0)
            {
                SqlCommand cmdCreateTable = new SqlCommand(@"
                        CREATE TABLE AllowedIPRanges (
                            ID INT PRIMARY KEY IDENTITY(1,1),
                            StartIPRange VARCHAR(15),
                            EndIPRange VARCHAR(15),
                            Description NVarChar(Max),
                            CreateDate DATETIME Default(GetDate()),
                            TenantId BIGINT
                        )", connection);

                cmdCreateTable.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}
