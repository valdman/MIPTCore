namespace FileManagment
{
    public class FileStorageSettings
    {
        public FileStorageSettings() 
        {
        }

        public string ImageStorageFolder { get;  set; }
        public string FileStorageFolder { get;  set; }
        public string[] AllowedImageExtensions { get;  set; }
        
        public string[] AllowedFileExtensions { get; set; }
        public int MaxImageSize { get;  set; }
    }
}
