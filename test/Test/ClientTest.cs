using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TonieCloud;

namespace Test
{
    public class ClientTest
    {
        private TonieCloudClient client;

        [SetUp]
        public void Setup()
        {
            DotNetEnv.Env.Load("../../../../../.env");
            
            client = new TonieCloudClient(new Login { Email = DotNetEnv.Env.GetString("MYTONIE_LOGIN"), Password = DotNetEnv.Env.GetString("MYTONIE_PASSWORD") });
        }

        [Test]
        public async Task GetCreativeTonies()
        {
            var households = await client.GetHouseholds();

            var tonies = await client.GetCreativeTonies(households[0].Id);

            var boxes = await client.GetTonieboxes(households[0].Id);
        }

        [Test]
        public async Task UploadFile()
        {
            var households = await client.GetHouseholds();
            var tonies = await client.GetCreativeTonies(households[0].Id);

            var tonie = tonies.First(t => t.Name == "Test");

            var uploadRequest = new UploadFilesToCreateiveTonieRequest
            {
                CreativeTonieId = tonie.Id,
                HouseholdId = households[0].Id,
                TonieName = "Test 2",
                Entries = new[]
                {
                    new UploadFilesToCreateiveTonieRequest.Entry
                    {
                        File = File.OpenRead("TestData/1.m4a"),
                        Name = "Kapitel 1"
                    }
                }
            };
            
            var response =  await client.UploadFilesToCreateiveTonie(uploadRequest);
        }
    }
}