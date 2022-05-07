namespace HW04
{
    public class ApplicationLogic
    {
        const string InputPath = "../../../../Data/";
        const string OutputPath = "../../../../Output/";

        public async static Task EncodeEverything(string[] imageNames, byte[][] chunks, int maxTasks)
        {
            using SemaphoreSlim sem = new SemaphoreSlim(maxTasks);

            var lambdaFunc = async (int i) =>
            {
                var processor = new StegoImageProcessor();
                // make sure we can work when get to the semaphor
                var image = await processor.LoadImageAsync(InputPath + imageNames[i]);

                await sem.WaitAsync();

                Console.WriteLine($"Chunk {i}, Thread: {Thread.CurrentThread.ManagedThreadId}");
                using var encodedImage = await processor.EncodePayload(image, chunks[i]);
                // We will start saving here, but we don't need to wait for it
                var saveRes = processor.SaveImageAsync(encodedImage, OutputPath + imageNames[i] + ".png");

                sem.Release();
                await saveRes;
            };

            var tasks = new Task[chunks.Length];
            for (int i = 0; i < chunks.Length; i++)
            {
                var tmp = i;
                tasks[i] = Task.Run(() => lambdaFunc(tmp));
            }

            // we need to wait here since we need to dispose of the semafor
            await Task.WhenAll(tasks);

            return;
        }

        public static async Task<byte[]> DecodeEverything(string[] imageNames, List<int> precomputedStats, int maxTasks)
        {
            byte[][] resultData = new byte[precomputedStats.Count][];

            using SemaphoreSlim sem = new SemaphoreSlim(maxTasks);

            var lambdaFunc = async (int i) =>
            {
                var processor = new StegoImageProcessor();
                // make sure we can work when get to the semaphor
                var image = await processor.LoadImageAsync(OutputPath + imageNames[i] + ".png");

                await sem.WaitAsync();

                Console.WriteLine($"Chunk {i}, Thread: {Thread.CurrentThread.ManagedThreadId}");
                // we need to await here since we have the limit on how many can work inside
                resultData[i] = await processor.ExtractPayload(image, precomputedStats[i]);

                sem.Release();
            };

            var tasks = new Task[precomputedStats.Count];
            for (int i = 0; i < precomputedStats.Count; i++)
            {
                var tmp = i;
                tasks[i] = Task.Run(() => lambdaFunc(tmp));
            }

            await Task.WhenAll(tasks);

            // Everything needs to be parrallel (even though this is probably slower)
            return resultData.AsParallel().AsOrdered().SelectMany(s => s.ToArray()).ToArray();
        }


    }
}
