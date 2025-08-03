namespace Mahat.Server.Models
{
    public class ForeignKeyTableCol : TableCol
    {
        public string ForeignKeyTableName { get; set; }
        public string ForeignKeyColName { get; set; }
    }
}
