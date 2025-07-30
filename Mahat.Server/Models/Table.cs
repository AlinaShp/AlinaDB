namespace Mahat.Server.Models
{
    public class Table
    {
        public string TableName { get; set; }
        public TableCol[] Cols { get; set; }

        public bool IsNotNull(Table table)
        {

            return !string.IsNullOrEmpty(table.TableName) &&
                   table.Cols != null && table.Cols.Length > 0 &&
                   table.Cols.All(col => !string.IsNullOrEmpty(col.ColName) &&
                                         !string.IsNullOrEmpty(col.DataType));
        }
    }
}