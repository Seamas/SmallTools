using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using ExcelDataReader;


/**
 * 读取Excel生成DDL
 */
namespace SQLCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var path = "/Users/seamas/Downloads/read.xlsx";
            var output = "/Users/seamas/Downloads/read.sql";
            var dataSet = ExcelUtil.ReadExcel(path);

            var sql = CreateDDL(dataSet, new OracleDDLCreator());

            File.WriteAllText(output, sql);
        }


        static string CreateDDL(DataSet dataSet, ICreator creator)
        {
            var builder = new StringBuilder();
                    
            foreach (DataTable dataTable in dataSet.Tables)
            {
                var table = CreateTable(dataTable);
                var ddl = creator.Create(table);
                builder.AppendLine(ddl);
            }

            return builder.ToString();
        }


        static Table CreateTable(DataTable dataTable)
        {
            var table = new Table();
            table.TableName = dataTable.TableName;
            table.Rows = new List<Field>();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                var row = new Field();
                
                var name = dataRow[0].ToString();
                var comment = dataRow[4].ToString();
                row.Name = name;
                row.DataType = dataRow[2].ToString();
                row.Comment = comment.StartsWith(name) ? comment : name + comment;
                table.Rows.Add(row);
            }
            return table;
        }
    }
}