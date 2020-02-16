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
            var fullPath = Path.Combine(LibraryRoot, path);

            var directory = System.IO.Directory.EnumerateDirectories(fullPath)
                .Select(p => new Directory
                {
                    Path = path + Path.DirectorySeparatorChar + Path.GetFileName(p),
                    Name = Path.GetFileName(p)
                })
                .ToArray();

            return directory;
        }
    }
}
