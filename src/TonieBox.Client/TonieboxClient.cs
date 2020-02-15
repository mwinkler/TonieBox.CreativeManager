using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TonieBox.Client;

namespace ToniBox.Client
{
    public class TonieboxClient
    {
        private const string TONIE_API_URL = "https://api.tonie.cloud";
        private const string AMAZON_UPLOAD_URL = "https://bxn-toniecloud-prod-upload.s3.amazonaws.com";
        private readonly Login login;
        private readonly HttpClient client;
        private readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

        public TonieboxClient(Login login)
        {
            this.login = login;

            client = new HttpClient
            {
                BaseAddress = new Uri(TONIE_API_URL),
            };
        }

        public Task<Household[]> GetHouseholds() => Get<Household[]>("/v2/households");
        
        public Task<CreativeTonie[]> GetCreativeTonies(string householdId) => Get<CreativeTonie[]>($"/v2/households/{householdId}/creativetonies");

        public Task<Toniebox[]> GetTonieboxes(string householdId) => Get<Toniebox[]>($"/v2/households/{householdId}/tonieboxes");

        public async Task<AmazonToken> UploadFile(Stream file)
        {
            // get upload token
            var amazonFile = await Post<AmazonToken>("/v2/file", new ByteArrayContent(new byte[] { }));

            // create payload
            var payload = new MultipartContent("form-data");
            payload.AddFormContent("key", amazonFile.Request.Fields.Key);
            payload.AddFormContent("x-amz-algorithm", amazonFile.Request.Fields.AmazonAlgorithm);
            payload.AddFormContent("x-amz-credential", amazonFile.Request.Fields.AmazonCredential);
            payload.AddFormContent("x-amz-date", amazonFile.Request.Fields.AmazonDate);
            payload.AddFormContent("policy", amazonFile.Request.Fields.Policy);
            payload.AddFormContent("x-amz-signature", amazonFile.Request.Fields.AmazonSignature);
            payload.AddStreamContent("file", amazonFile.FileId, file, "application/octet-stream");

            // upload to S3
            var response = await new HttpClient().PostAsync(AMAZON_UPLOAD_URL, payload);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error while upload to Amazon S3");
            }

            return amazonFile;
        }

        public Task PatchCreativeTonie(string householdId, string creativeTonieId, string name, IEnumerable<Chapter> chapters)
        {
            var payload = new 
            { 
                chapters = chapters,
                name = name
            };

            return Patch($"/v2/households/{householdId}/creativetonies/{creativeTonieId}", payload);
        }

        private async Task UpdateJwtToken()
        {
            var response = await client.PostAsync("/v2/sessions", new StringContent(JsonConvert.SerializeObject(login, jsonSettings) , Encoding.UTF8, "application/json"));

            var token = JsonConvert.DeserializeObject<JwtToken>(await response.Content.ReadAsStringAsync());

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Jwt);
        }

        private Task<T> Get<T>(string path) => ExecuteRequest<T>(() => client.GetAsync(path));
        
        private Task<T> Post<T>(string path, HttpContent content) => ExecuteRequest<T>(() => client.PostAsync(path, content));

        private Task Patch(string path, object content)
        {
            var payload = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

            return ExecuteRequest<object>(() => client.PatchAsync(path, payload));
        }

        private async Task<T> ExecuteRequest<T>(Func<Task<HttpResponseMessage>> action)
        {
            if (client.DefaultRequestHeaders.Authorization == null)
            {
                await UpdateJwtToken();
            }

            var response = await action.Invoke();

            // refresh jwt token in case of 401
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await UpdateJwtToken();

                response = await action.Invoke();
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Request failed with {response.StatusCode}");
            }

            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }
    }
}
