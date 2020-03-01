
namespace TonieBox.Client
{
    public class CreativeTonie
    {
        public class Transcodingerror
        {
            public string Reason { get; set; }
            public Deletedchapter[] DeletedChapters { get; set; }
            public string Message { get; set; }
        }

        public class Deletedchapter
        {
            public string Title { get; set; }
            public float Seconds { get; set; }
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public bool Live { get; set; }
        public bool Private { get; set; }
        public string ImageUrl { get; set; }
        public Transcodingerror[] TranscodingErrors { get; set; }
        public Chapter[] Chapters { get; set; }
        public bool Transcoding { get; set; }
        public float SecondsPresent { get; set; }
        public float SecondsRemaining { get; set; }
        public int ChaptersPresent { get; set; }
        public int ChaptersRemaining { get; set; }
    }
}
