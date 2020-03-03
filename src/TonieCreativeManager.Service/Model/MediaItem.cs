
namespace TonieCreativeManager.Service.Model
{
    public class MediaItem
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public byte[] Cover { get; set; }
        public bool HasSubitems { get; set; }
        public string MappedTonieId { get; set; }
    }
}
