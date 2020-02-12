using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ToniBox.Client
{
    public class TonieboxClient
    {
        private readonly Login login;
        private readonly HttpClient client;
        private readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

        public TonieboxClient(Login login)
        {
            this.login = login;

            client = new HttpClient
            {
                BaseAddress = new Uri("https://api.tonie.cloud"),
            };
        }

        public Task<Household[]> GetHouseholds() => Get<Household[]>("/v2/households");
        
        public Task<CreativeTonie[]> GetCreativeTonies(string householdId) => Get<CreativeTonie[]>($"/v2/households/{householdId}/creativetonies");

        public Task<Toniebox[]> GetTonieboxes(string householdId) => Get<Toniebox[]>($"/v2/households/{householdId}/tonieboxes");

        private async Task UpdateJwtToken()
        {
            var response = await client.PostAsync("/v2/sessions", new StringContent(JsonConvert.SerializeObject(login, jsonSettings) , Encoding.UTF8, "application/json"));

            var token = JsonConvert.DeserializeObject<JwtToken>(await response.Content.ReadAsStringAsync());

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Jwt);
        }

        private async Task<T> Get<T>(string path)
        {
            if (client.DefaultRequestHeaders.Authorization == null)
            {
                await UpdateJwtToken();
            }

            var response = await client.GetAsync(path);

            // try refresh jwt token
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await UpdateJwtToken();

                response = await client.GetAsync(path);
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"GET request '{path}' failed with {response.StatusCode}");
            }

            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }
    }
}
