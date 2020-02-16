using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TonieBox.Service;

namespace TonieBox.Ui.Pages
{
    public partial class Index
    {
        [Inject] private FileService FileService { get; set; }

        private IEnumerable<Directory> Directories;

        private Directory Selected;

        protected override async Task OnInitializedAsync()
        {
            Directories = await FileService.GetDirectory(null);
        }

        async Task FolderClick(Directory directory)
        {
            Selected = directory;

            if (directory.HasSubfolders)
            {
                Directories = await FileService.GetDirectory(directory.Path);

            }
            else
            {

            }

        }

        async Task ParentClick()
        {
            Directories = await FileService.GetDirectory(Selected.ParentPath);
        }
    }
}
