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

        public async Task<IEnumerable<MediaItem>> GetItems(string path)
        {
            var fullPath = settings.LibraryRoot + path;
            var mappings = await repositoryService.GetMappings();

            bool isNotIgnoredFolder(string p) => !settings.IgnoreFolderNames.Contains(Path.GetFileName(p), StringComparer.OrdinalIgnoreCase);

            var directory = Directory.GetDirectories(fullPath)
                .Where(isNotIgnoredFolder)
                .Select(fullpath => 
                {
                    var subpath = path + "/" + Path.GetFileName(fullpath);
                    var hasSubitems = Directory.GetDirectories(fullpath).Where(isNotIgnoredFolder).Any();

                    return new MediaItem
                    {
                        Path = subpath,
                        Name = Path.GetFileName(fullpath),
                        HasSubitems = hasSubitems,
                        MappedTonieId = mappings.FirstOrDefault(m => m.Path == subpath)?.TonieId,
                        HasBought = settings.EnableShop 
                            ? hasSubitems
                                ? Directory.GetFiles(fullpath, settings.MarkAsBoughtFilename, SearchOption.AllDirectories).Any()
                                : File.Exists(fullpath + "/" + settings.MarkAsBoughtFilename)
                            : true
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

        public Task MarkFolderAsBought(string path)
        {
            var full = settings.LibraryRoot + path + "/" + settings.MarkAsBoughtFilename;

            return File.WriteAllTextAsync(full, "");
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
