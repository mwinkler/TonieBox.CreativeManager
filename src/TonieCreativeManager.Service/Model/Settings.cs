using System.Collections.Generic;

namespace TonieCreativeManager.Service.Model
{
    public class Settings
    {
        public IEnumerable<string> SupportedFileExtensions { get; set; }
        public IEnumerable<string> FolderCoverFiles { get; set; }
        public string LibraryRoot { get; set; }
        public IEnumerable<string> IgnoreFolderNames { get; set; }
        public string PersistentDataFile { get; set; }
        public bool EnableShop { get; set; }
        public IEnumerable<string> KeyboardCharacters { get; set; }
    }
}
