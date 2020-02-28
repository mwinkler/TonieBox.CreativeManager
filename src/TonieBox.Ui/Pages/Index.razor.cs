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
        [Inject] private TonieboxService TonieboxService { get; set; }

        private IEnumerable<Directory> Directories;
        private Directory Selected;
        private IEnumerable<CreativeTonie> Tonies;
        private Household Household;
        private bool Uploading;

        protected override async Task OnInitializedAsync()
        {
            Directories = await FileService.GetDirectories(null);
            Household = (await TonieboxService.GetHouseholds()).First();
        }

        async Task FolderClick(Directory directory)
        {
            Selected = directory;

            if (directory.HasSubfolders)
            {
                Directories = await FileService.GetDirectories(directory.Path);
            }
            else
            {
                Tonies = await TonieboxService.GetCreativeTonies(Household.Id);
            }
        }

        async Task ParentClick()
        {
            //Directories = await FileService.GetDirectories(Selected.ParentPath);
        }

        void Back()
        {
            Tonies = null;
        }

        async Task Upload(CreativeTonie tonie)
        {
            Uploading = true;
            StateHasChanged();

            await TonieboxService.Upload(Selected.Path, Household.Id, tonie.Id);

            Selected = null;
            Tonies = null;
            Uploading = false;
        }
    }
}
