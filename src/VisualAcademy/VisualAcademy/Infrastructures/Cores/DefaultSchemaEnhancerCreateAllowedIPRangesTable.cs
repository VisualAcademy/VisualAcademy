namespace All.Infrastructures.Cores;

public class DefaultSchemaEnhancerCreateAllowedIpRangesTable(string defaultConnectionString)
{
    public void EnhanceDefaultDatabase() => CreateAllowedIpRangesTableIfNotExists();

    private void CreateAllowedIpRangesTableIfNotExists()
    {
        using (SqlConnection connection = new SqlConnection(defaultConnectionString))
        {
            connection.Open();

            SqlCommand cmdCheck = new SqlCommand(@"
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_SCHEMA = 'dbo' 
                    AND TABLE_NAME = 'AllowedIpRanges'", connection);

            int tableCount = (int)cmdCheck.ExecuteScalar();

            if (tableCount == 0)
            {
                SqlCommand cmdCreateTable = new SqlCommand(@"
                        CREATE TABLE AllowedIpRanges (
                            ID INT PRIMARY KEY IDENTITY(1,1),
                            StartIpRange VARCHAR(15),
                            EndIpRange VARCHAR(15),
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
