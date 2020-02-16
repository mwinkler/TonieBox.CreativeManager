using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TonieBox.Service
{
    public class FileService
    {
        private readonly string LibraryRoot;

        public FileService(IConfiguration config)
        {
            LibraryRoot = config["LibraryRoot"];
        }

        public async Task<IEnumerable<Directory>> GetDirectory(string path)
        {
            var fullPath = LibraryRoot + path;

            var directory = System.IO.Directory.GetDirectories(fullPath)
                .Select(p => new Directory
                {
                    Path = path + Path.DirectorySeparatorChar + Path.GetFileName(p),
                    Name = Path.GetFileName(p),
                    ParentPath = Path.GetDirectoryName(path),
                    HasSubfolders = System.IO.Directory.GetDirectories(p).Any()
                })
                .ToArray();

            return directory;
        }
    }
}
