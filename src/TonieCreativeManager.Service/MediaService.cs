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
        private readonly RepositoryService repositoryService;

        public MediaService(Settings settings, RepositoryService repositoryService)
        {
            Console.WriteLine($"Using '{settings.LibraryRoot}' for library root");

            this.settings = settings;
            this.repositoryService = repositoryService;
        }

        public Task<IEnumerable<MediaItem>> GetItems(string path)
        {
            var fullPath = settings.LibraryRoot + path;
            var mappings = repositoryService.GetMappings();

            bool isNotIgnoredFolder(string p) => !settings.IgnoreFolderNames.Contains(Path.GetFileName(p), StringComparer.OrdinalIgnoreCase);

            var directory = Directory.GetDirectories(fullPath)
                .Where(isNotIgnoredFolder)
                .Select(p => 
                {
                    var subpath = path + "/" + Path.GetFileName(p);

                    return new MediaItem
                    {
                        Path = subpath,
                        Name = Path.GetFileName(p),
                        HasSubitems = Directory.GetDirectories(p).Where(isNotIgnoredFolder).Any(),
                        MappedTonieId = mappings.FirstOrDefault(m => m.Path == subpath)?.TonieId
                    };
                })
                .OrderBy(p => p.Name)
                .ToArray();

            return Task.FromResult(directory.AsEnumerable());
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
            var files = Directory.GetFiles(fullPath);

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
