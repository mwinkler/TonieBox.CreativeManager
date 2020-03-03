
namespace TonieCreativeManager.Service.Model
{
    public class Directory
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public byte[] Cover { get; set; }
        public bool HasSubfolders { get; set; }
        public string MappedTonieId { get; set; }
    }
}
