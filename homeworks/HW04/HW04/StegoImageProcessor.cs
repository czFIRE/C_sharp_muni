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
            // I wanna throw this, but this is more like "programmer, you are doing it wrong" vs "aaaaa, user bad, bad input"
            // Thus I should control it outside of here, but ...
            if (payload.Length > image.Width * image.Height - 1)
            {
                throw new ArgumentException("Can't encode the message in this image!");
            }

            // Get compression ratio and store it in the first byte
            byte compressionRatio = BitsInByte;
            while(payload.Length * compressionRatio > image.Width * image.Height - 1)
            {
                compressionRatio /= 2;
            }

            // This can be CPU-intensive, so it can run in separate task
            Rgba32[] pixelArray = new Rgba32[image.Width * image.Height];
            image.CopyPixelDataTo(pixelArray);

            // we will keep our compression ratio for each image separatelly - and it will be stored on the last 2 bits of the last element
            // and yes, we need these temporarry variables else it won't work
            var tmp1 = pixelArray[image.Width * image.Height - 1].B;
            var tmp2 = tmp1 & 0xfc;
            var tmp3 = (byte)Math.Log2(compressionRatio);

            pixelArray[image.Width * image.Height - 1].B = (byte) (tmp2 + tmp3);
                                
            byte maskKeep = (byte) (0xff - ((1 << (BitsInByte / compressionRatio)) - 1));

            for (int i = 0; i < payload.Length; i++)
            {
                var splitByte = ByteSpliting.Split(payload[i], BitsInByte / compressionRatio).ToArray();
                for (int j = 0; j < compressionRatio; j++)
                {
                    int tmp = pixelArray[i * compressionRatio + j].B & maskKeep;
                    pixelArray[i*compressionRatio + j].B = (byte)(tmp + splitByte[j]);
                }
            }

            image.Dispose();

            return Image.LoadPixelData<Rgba32>(pixelArray, image.Width, image.Height);
        });

        public Task<byte[]> ExtractPayload(Image<Rgba32> image, int dataSize) => Task.Run(() =>
        {
            // This can be CPU-intensive, so it can run in separate task
            Rgba32[] pixelArray = new Rgba32[image.Width * image.Height];
            image.CopyPixelDataTo(pixelArray);

            var helpTmp = pixelArray[image.Width * image.Height - 1].B;
            byte compressionRatio = (byte) (1 << (helpTmp & 0x3));

            var mask = ((1 << (BitsInByte / compressionRatio)) - 1);

            var res = new byte[dataSize];
            var helperArr = new byte[compressionRatio];

            for (int i = 0; i < dataSize; i++)
            {
                for (int j = 0; j < compressionRatio; j++)
                {
                    helperArr[j] = (byte) (pixelArray[i * compressionRatio + j].B & mask);
                }

                res[i] = ByteSpliting.Reform(helperArr, BitsInByte / compressionRatio);
            }

            image.Dispose();

            return res;
        });
    }
}
