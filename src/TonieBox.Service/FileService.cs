using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace TonieBox.Service
{
    public class FileService
    {
        private static readonly string[] ImageExtensions = new string[] { ".png", ".jpg", ".jpeg", ".gif" };
        private readonly Settings settings;

        public FileService(Settings settings)
        {
            Console.WriteLine($"Using '{settings.LibraryRoot}' for library root");

            this.settings = settings;
        }

        public Task<IEnumerable<Directory>> GetDirectories(string path)
        {
            var fullPath = settings.LibraryRoot + path;

            var directory = System.IO.Directory.GetDirectories(fullPath)
                .Where(p => !settings.IgnoreFolderNames.Contains(Path.GetDirectoryName(p), StringComparer.OrdinalIgnoreCase))
                .Select(p => new Directory
                {
                    Path = path + Path.DirectorySeparatorChar + Path.GetFileName(p),
                    Name = Path.GetFileName(p),
                    HasSubfolders = System.IO.Directory.GetDirectories(p).Any()
                })
                .ToArray();

            return Task.FromResult((IEnumerable<Directory>)directory);
        }

        public Task<Cover> GetDirectoryCover(string path)
        {
            if (path != "folder")
            {
                var fullPath = settings.LibraryRoot + path;
                var files = System.IO.Directory.GetFiles(fullPath);

                // specific cover files
                var coverFile = files.FirstOrDefault(p => settings.FolderCoverFiles.Contains(Path.GetFileName(p), StringComparer.OrdinalIgnoreCase));

                if (coverFile != null)
                {
                    return Task.FromResult(new Cover
                    {
                        Data = File.OpenRead(coverFile),
                        MimeType = "application/octet-stream"
                    });
                }

                // any image files
                var imageFile = files.FirstOrDefault(p => ImageExtensions.Contains(Path.GetExtension(p), StringComparer.OrdinalIgnoreCase));
            
                if (imageFile != null)
                {
                    return Task.FromResult(new Cover
                    {
                        Data = File.OpenRead(imageFile),
                        MimeType = "application/octet-stream"
                    });
                }
            }

            // default folder image
            return Task.FromResult(new Cover
            {
                Data = typeof(FileService).GetTypeInfo().Assembly.GetManifestResourceStream("TonieBox.Service.folder.png"),
                MimeType = "image/png"
            });
        }
    }
}
