using Mahat.Server.Enums;


namespace Mahat.Server.Models
{
    public class BackupInfo
    {
        public BackupTypes BackupType { get; set; }
        public string FilePath { get; set; }

        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (value.Contains('.'))
                {
                    throw new ArgumentException("File name can't contain dots");
                }
                _fileName = value;
            }
        }

        public bool IsNotNull(BackupInfo backupInfo)
        {
            return !string.IsNullOrEmpty(backupInfo.FilePath) && 
                   !string.IsNullOrEmpty(backupInfo.FileName) && 
                   !string.IsNullOrEmpty(backupInfo.BackupType.ToString());
        }
    }
}
