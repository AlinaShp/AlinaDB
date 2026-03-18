namespace Mahat.Server.DTOs
{
    public class TableColDto
    {
        public string ColName { get; set; }
        public string DataType { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsNullable { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsUnique { get; set; }
    }
}
