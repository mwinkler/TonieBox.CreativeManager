﻿using Microsoft.Extensions.Configuration;
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
            this.settings = settings;
        }

        public async Task<IEnumerable<Directory>> GetDirectory(string path)
        {
            var fullPath = settings.LibraryRoot + path;

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

        public async Task<Cover> GetDirectoryCover(string path)
        {
            var fullPath = settings.LibraryRoot + path;
            var files = System.IO.Directory.GetFiles(fullPath);

            // specific cover files
            var coverFile = files.FirstOrDefault(p => settings.FolderCoverFiles.Contains(Path.GetFileName(p), StringComparer.OrdinalIgnoreCase));

            if (coverFile != null)
            {
                return new Cover
                {
                    Data = File.OpenRead(coverFile),
                    MimeType = "application/octet-stream"
                };
            }

            // any image files
            var imageFile = files.FirstOrDefault(p => ImageExtensions.Contains(Path.GetExtension(p), StringComparer.OrdinalIgnoreCase));
            
            if (imageFile != null)
            {
                return new Cover
                {
                    Data = File.OpenRead(imageFile),
                    MimeType = "application/octet-stream"
                };
            }

            // default folder image
            return new Cover
            {
                Data = typeof(FileService).GetTypeInfo().Assembly.GetManifestResourceStream("TonieBox.Service.folder.png"),
                MimeType = "image/png"
            };
        }
    }
}