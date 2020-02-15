using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToniBox.Client;

namespace TonieBox.Client
{
    public class TonieboxService
    {
        private readonly TonieboxClient client;

        public TonieboxService(TonieboxClient client)
        {
            this.client = client;
        }

        public async Task UploadFilesToCreateiveTonie(CreativeTonieUploadRequest request)
        {
            // upload files
            async Task uploadFile(CreativeTonieUploadRequest.Entry entry)
            {
                var fileToken = await client.UploadFile(entry.File);
            }

            //Parallel.ForEach(request.Entries
        }
    }
}
