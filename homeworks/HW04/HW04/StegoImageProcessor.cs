using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace HW04
{
    public class StegoImageProcessor
    {
        const int BitsInByte = 8;

        // Use constructor for additional configuration

        public async Task<Image<Rgba32>> LoadImageAsync(string path) => await Image.LoadAsync<Rgba32>(path).ConfigureAwait(false);

        public Task SaveImageAsync(Image<Rgba32> image, string path) => Task.Run(async () => { await image.SaveAsync(path).ConfigureAwait(false); });

        public Task<Image<Rgba32>> EncodePayload(Image<Rgba32> image, byte[] payload) => Task.Run(() => 
        {
            // This can be CPU-intensive, so it can run in separate task
            Rgba32[] pixelArray = new Rgba32[image.Width * image.Height];
            image.CopyPixelDataTo(pixelArray);

            for (int i = 0; i < payload.Length; i++)
            {
                pixelArray[i].B = payload[i];
            }

            return Image.LoadPixelData<Rgba32>(pixelArray, image.Width, image.Height);
        });

        public Task<byte[]> ExtractPayload(Image<Rgba32> image, int dataSize) => Task.Run(() =>
        {
            // This can be CPU-intensive, so it can run in separate task
            Rgba32[] pixelArray = new Rgba32[image.Width * image.Height];
            image.CopyPixelDataTo(pixelArray);

            var res = new byte[dataSize];

            for (int i = 0; i < dataSize; i++)
            {
                res[i] = pixelArray[i].B;
            }

            return res;
        });
    }
}
