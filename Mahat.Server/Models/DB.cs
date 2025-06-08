namespace Mahat.Server.Models
{
    public class DB
    {
        public string DatabaseName { get; set; }
        public int DatabaseId { get; set; }
        public string State { get; set; }
        public string RecoveryModel { get; set; }
        public int CompatibilityLevel { get; set; }
        public string Collation { get; set; }
        public List<string> Tables { get; set; }= new List<string>();
    }
}