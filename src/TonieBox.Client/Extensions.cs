using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TonieBox.Client
{
    internal static class Extensions
    {
        internal static void AddFormContent(this MultipartContent multipart, string name, string content)
        {
            multipart.Add(new StringContent(content)
            {
                Headers =
                {
                    ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = name
                    }
                }
            });
        }

        internal static void AddStreamContent(this MultipartContent multipart, string name, string filename, Stream stream, string contentType)
        {
            multipart.Add(new StreamContent(stream)
            {
                Headers =
                {
                    ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = name,
                        FileName = filename,
                    },
                    ContentType = new MediaTypeHeaderValue(contentType)
                }
            });
        }
    }
}
