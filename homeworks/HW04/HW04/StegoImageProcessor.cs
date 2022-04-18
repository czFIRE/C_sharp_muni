using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace HW04
{
    public class StegoImageProcessor
    {
        const int BitsInByte = 8;

        // Use constructor for additional configuration

        public async Task<Image<Rgba32>> LoadImageAsync(string path) => throw new NotImplementedException();

        public Task SaveImageAsync(Image<Rgba32> image, string path) => throw new NotImplementedException();

        public Task<Image<Rgba32>> EncodePayload(Image<Rgba32> image, byte[] payload) => Task.Run(() => 
        {
            // This can be CPU-intensive, so it can run in separate task
            throw new NotImplementedException();
            return image;
        });

        public Task<byte[]> ExtractPayload(Image<Rgba32> image, int dataSize) => Task.Run(() =>
        {
            // This can be CPU-intensive, so it can run in separate task
            throw new NotImplementedException();
            return Array.Empty<byte>();
        });
    }
}
