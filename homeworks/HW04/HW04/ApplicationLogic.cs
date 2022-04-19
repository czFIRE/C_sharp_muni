namespace HW04
{
    public class ApplicationLogic
    {
        const string inputPath = "../../../../Data/";
        const string outputPath = "../../../../Output/";

        public static Task encodeEverything(string[] imageNames, byte[][] chunks, int maxTasks)
        {
            var processor = new StegoImageProcessor();

            var tasks = new Task[chunks.Length];

            var lambdaFunc = async (int i) =>
            {
                Console.WriteLine($"Chunk {i}, Thread: {Thread.CurrentThread.ManagedThreadId}");
                using var encodedImage = await processor.EncodePayload(await processor.LoadImageAsync(inputPath + imageNames[i]), chunks[i]);
                await processor.SaveImageAsync(encodedImage, outputPath + imageNames[i] + ".png");
            };

            for (int i = 0; i < chunks.Length; i++)
            {
                var tmp = i;
                tasks[i] = Task.Run(() => lambdaFunc(tmp));
            }

            return Task.WhenAll(tasks);
        }

        public async static Task<byte[]> decodeEverything(string[] imageNames, byte[][] chunks, int maxTasks)
        {
            var processor = new StegoImageProcessor();

            var tasks = new Task[chunks.Length];
            byte[][] resultData = new byte[chunks.Length][];

            var lambdaFunc = async (int i) =>
            {
                Console.WriteLine($"Chunk {i}, Thread: {Thread.CurrentThread.ManagedThreadId}");
                resultData[i] = await processor.ExtractPayload(await processor.LoadImageAsync(outputPath + imageNames[i] + ".png"), chunks[i].Length);
            };

            for (int i = 0; i < chunks.Length; i++)
            {
                var tmp = i;
                tasks[i] = Task.Run(() => lambdaFunc(tmp));
            }

            await Task.WhenAll(tasks);

            return resultData.SelectMany(s => s.ToArray()).ToArray();
        }


    }
}
