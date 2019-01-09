using System.Collections.Generic;

namespace SQLCreator
{
    public class Table
    {
        public string TableName { get; set; }
        public List<Field> Rows { get; set; }
    }
}