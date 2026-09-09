namespace DevLogix.Services;

using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

public class ExportService : IExportService
{
    private readonly string _connectionString;

    public ExportService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found.");
    }

    public async Task<byte[]> ExportTicketsToCsvAsync(int? projectId = null)
    {
        var dt = new DataTable();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            
            var query = @"
                SELECT 
                    t.Id,
                    t.Title,
                    t.Status,
                    t.Priority,
                    p.Title as ProjectName,
                    u.FullName as CreatedBy
                FROM Tickets t
                LEFT JOIN Projects p ON t.ProjectId = p.Id
                LEFT JOIN AspNetUsers u ON t.CreatedById = u.Id
                WHERE (@ProjectId IS NULL OR t.ProjectId = @ProjectId)
                ORDER BY t.Id DESC";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ProjectId", projectId.HasValue ? (object)projectId.Value : DBNull.Value);

            using var adapter = new SqlDataAdapter(command);
            adapter.Fill(dt);
        }

        var sb = new StringBuilder();
        
        // Headers
        var columnNames = dt.Columns.Cast<DataColumn>().Select(column => EscapeCsvValue(column.ColumnName));
        sb.AppendLine(string.Join(",", columnNames));

        // Rows
        foreach (DataRow row in dt.Rows)
        {
            var fields = row.ItemArray.Select(field => EscapeCsvValue(field?.ToString() ?? string.Empty));
            sb.AppendLine(string.Join(",", fields));
        }

        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    private string EscapeCsvValue(string value)
    {
        if (value.Contains(",") || value.Contains("\"") || value.Contains("\r") || value.Contains("\n"))
        {
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }
        return value;
    }
}
