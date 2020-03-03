using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TonieCreativeManager.Service.Model;

namespace TonieCreativeManager.Service
{
    public class MediaService
    {
        private static readonly string[] ImageExtensions = new string[] { ".png", ".jpg", ".jpeg", ".gif" };
        private readonly Settings settings;
        private readonly MappingService mappingService;

        public MediaService(Settings settings, MappingService mappingService)
        {
            Console.WriteLine($"Using '{settings.LibraryRoot}' for library root");

            this.settings = settings;
            this.mappingService = mappingService;
        }

        public async Task<IEnumerable<Model.Directory>> GetDirectories(string path)
        {
            var fullPath = settings.LibraryRoot + path;
            var mappings = await mappingService.GetMappings();

            bool isNotIgnoredFolder(string p) => !settings.IgnoreFolderNames.Contains(Path.GetFileName(p), StringComparer.OrdinalIgnoreCase);

            var directory = System.IO.Directory.GetDirectories(fullPath)
                .Where(isNotIgnoredFolder)
                .Select(p => 
                {
                    var subpath = path + "/" + Path.GetFileName(p);

                    return new Model.Directory
                    {
                        Path = subpath,
                        Name = Path.GetFileName(p),
                        HasSubfolders = System.IO.Directory.GetDirectories(p).Where(isNotIgnoredFolder).Any(),
                        MappedTonieId = mappings.FirstOrDefault(m => m.Path == subpath)?.TonieId
                    };
                })
                .OrderBy(p => p.Name)
                .ToArray();

            return directory;
        }

        public async Task<Cover> GetCover(string path)
        {
            if (path != "folder")
            {
                while (true)
                {
                    var coverPath = await TryGetCoverPath(path);

                    if (coverPath != null)
                    {
                        return new Cover
                        {
                            Data = File.OpenRead(coverPath),
                            MimeType = "application/octet-stream"
                        };
                    }

                    // switch to parent
                    path = path.GetParentPath();

                    if (string.IsNullOrEmpty(path))
                    {
                        break;
                    }
                }
            }

            // default folder image
            return new Cover
            {
                Data = typeof(MediaService).GetTypeInfo().Assembly.GetManifestResourceStream("TonieCreativeManager.Service.folder.png"),
                MimeType = "image/png"
            };
        }

        private Task<string> TryGetCoverPath(string path)
        {
            var fullPath = settings.LibraryRoot + path;
            var files = System.IO.Directory.GetFiles(fullPath);

            // specific cover files
            var coverFile = files.FirstOrDefault(p => settings.FolderCoverFiles.Contains(Path.GetFileName(p), StringComparer.OrdinalIgnoreCase));

            if (coverFile != null)
            {
                return Task.FromResult(coverFile);
            }

            // any image files
            var imageFile = files.FirstOrDefault(p => ImageExtensions.Contains(Path.GetExtension(p), StringComparer.OrdinalIgnoreCase));

            if (imageFile != null)
            {
                return Task.FromResult(imageFile);
            }

            return Task.FromResult<string>(null);
        }
    }
}
