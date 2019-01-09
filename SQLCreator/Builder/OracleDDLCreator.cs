using System.Text;

namespace SQLCreator
{
    public class OracleDDLCreator: DDLCreator
    {
        protected override string CreateComment(Table table)
        {
            var builder = new StringBuilder();
            // COMMENT TABLENAME
            builder.AppendLine($"COMMENT ON TABLE {table.TableName} IS '{table.TableName}';");
            
            // COMMENT FIELD
            foreach (var row in table.Rows)
            {
                builder.AppendLine($"COMMENT ON COLUMN {table.TableName}.{row.Name} IS '{row.Comment}';");
            }
            return builder.ToString();
        }
    }
}