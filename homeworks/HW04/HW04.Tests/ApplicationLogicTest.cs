using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HW04.Tests
{
    public class ApplicationLogicTest
    {
        [Fact]
        public async Task EncodePayload_ExtractPayload_OneImage_Match()
        {
            // Well this is broken


            var imageNames = new[]
            {
                "John_Martin_-_Belshazzar's_Feast.jpg",
                "John_Martin_-_Pandemonium.jpg",
                "John_Martin_-_Sodom_and_Gomorrah.jpg",
                "John_Martin_-_The_Great_Day_of_His_Wrath.jpg",
                "John_Martin_-_The_Last_Judgement.jpg",
                "John_Martin_-_The_Plains_of_Heaven.jpg",
            };

            var chunks = StegoObject.LoadObject(Samples.StringSample(), (s) => Encoding.Default.GetBytes(s)).GetDataChunks(imageNames.Length).ToArray();

            List<int> precomputedStats = chunks.Select(s => s.Length).ToList();

            int maxTasks = imageNames.Length;

            // This will fail on multiple accesses to a file :(
            int paralelTestLength = 1;

            var tasks = new Task[paralelTestLength];
            for (int i = 0; i < paralelTestLength; i++)
            {
                var tmp = i;
                tasks[i] = Task.Run(async () =>
                {
                    await ApplicationLogic.EncodeEverything(imageNames, chunks, maxTasks);

                    byte[] decodedData = await ApplicationLogic.DecodeEverything(imageNames, precomputedStats, maxTasks);

                    string resString = Encoding.Default.GetString(decodedData);

                    Assert.Equal(resString, Samples.StringSample());
                });
            }

            await Task.WhenAll(tasks);
        }
    }
}
