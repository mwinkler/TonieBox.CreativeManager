using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TonieBox.Client;
using TonieBox.Service;

namespace TonieBox.Ui.Pages
{
    public partial class Index
    {
        [Inject] private FileService FileService { get; set; }
        [Inject] private TonieboxClient TonieboxClient { get; set; }

        private IEnumerable<Directory> Directories;
        private Directory Selected;
        private IEnumerable<CreativeTonie> Tonies;
        private Household Household;

        protected override async Task OnInitializedAsync()
        {
            Directories = await FileService.GetDirectory(null);
            Household = (await TonieboxClient.GetHouseholds()).First();
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
                
                Tonies = await TonieboxClient.GetCreativeTonies(Household.Id);
            }

        }

        async Task ParentClick()
        {
            Directories = await FileService.GetDirectory(Selected.ParentPath);
        }
    }
}
