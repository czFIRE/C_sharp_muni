namespace HW04
{
    public class ByteSpliting
    {
        const int BitsInByte = 8;

        /// <summary>
        /// Splits the byte to chunks of given size.
        /// Mind the endianness! The least significant chunks are on lower index.
        /// </summary>
        /// <param name="byte">byte to split</param>
        /// <param name="size">bits in each chunk</param>
        /// <example>Split(207,2) => [3,3,0,3]</example>
        /// <returns>chunks</returns>
        public static IEnumerable<byte> Split(byte @byte, int size)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reforms chunks to a byte.
        /// Mind the endianness! The least significant chunks are on lower index.
        /// </summary>
        /// <param name="parts">chunks to reform</param>
        /// <param name="size">bits in each chunk</param>
        /// <example>Split([3,3,0,3],2) => 207</example>
        /// <returns>byte</returns>
        public static byte Reform(IEnumerable<byte> parts, int size)
        {
            throw new NotImplementedException();
        }
    }
}