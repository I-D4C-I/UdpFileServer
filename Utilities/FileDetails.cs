namespace UdpFileServer.Utilities
{
    [Serializable]
    internal class FileDetails
    {
        public string FileType { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }

        public FileDetails()
        {
            FileName = "NoName";
            FileType = "NoType";
            FileSize = 0;
        }
    }
}
