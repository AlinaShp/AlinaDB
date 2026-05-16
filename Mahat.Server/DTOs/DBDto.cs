namespace Mahat.Server.DTOs
{
    public class DBDto
    {
        public string DatabaseName { get; set; }
        public string Collation { get; set; }

        public bool IsNotNull(DBDto dbDto)
        {
            return !string.IsNullOrEmpty(dbDto.DatabaseName) &&
                   !string.IsNullOrEmpty(dbDto.Collation);
        }
    }
}
