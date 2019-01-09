using System;
using System.Text;

namespace SQLCreator
{
    public abstract class DDLCreator: ICreator
    {
        public virtual string Create(Table table)
        {
            var builder = new StringBuilder();
            builder.Append(CreateTable(table));

            builder.AppendLine();
            
            builder.Append(CreateComment(table));
            return builder.ToString();
        }


        protected virtual string CreateTable(Table table)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"CREATE TABLE {table.TableName} (");

            for (var index = 0; index < table.Rows.Count; index++)
            {
                builder.Append($"\t{table.Rows[index].Name, -30}{table.Rows[index].DataType}")
                    .Append(table.Rows[index].NotNull ? " NOT NULL" : "")
                    .AppendLine(index == table.Rows.Count - 1 ? "" : ",");
            }

            builder.AppendLine(");");
            return builder.ToString();
        }

        protected abstract string CreateComment(Table table);
    }
}