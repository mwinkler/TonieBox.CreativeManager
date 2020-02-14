using NUnit.Framework;
using System.IO;
using System.Threading.Tasks;
using ToniBox.Client;

namespace Test
{
    public class ClientTest
    {
        private TonieboxClient client;

        [SetUp]
        public void Setup()
        {
            DotNetEnv.Env.Load("../../../../../.env");
            
            client = new TonieboxClient(new Login { Email = DotNetEnv.Env.GetString("MYTONIE_LOGIN"), Password = DotNetEnv.Env.GetString("MYTONIE_PASSWORD") });
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
            await client.UploadFile(File.OpenRead("TestData/1.m4a"));
        }
    }
}