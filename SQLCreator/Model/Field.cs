namespace SQLCreator
{
    public class Field
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public string Comment { get; set; }
        public bool NotNull { get; set; }
        public bool Key { get; set; }
    }
}