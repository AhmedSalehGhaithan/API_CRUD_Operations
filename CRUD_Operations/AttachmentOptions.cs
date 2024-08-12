namespace CRUD_Operations
{
    public class AttachmentOptions
    {
        public string AllowedExtensions { get; set; }
        public int MaxSizeInMegaBytes { get; set; }
        public bool EnableCompression { get; set; }
    }
}
