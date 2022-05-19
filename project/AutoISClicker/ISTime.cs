using System.Net;
using System.Net.Sockets;

namespace AutoISClicker
{
    public class ISTime
    {
        private TimeSpan TimeDifference;

        public ISTime()
        {
            TimeDifference = GetNetworkTimeDifference();
        }

        private TimeSpan GetNetworkTimeDifference()
        {
            while (!Utilities.AccessLock(1)) { }

            // FI MUNI time server
            const string ntpServer = "time.fi.muni.cz";

            // NTP message size - 16 bytes of the digest (RFC 2030)
            var ntpData = new byte[48];

            // Setting the Leap Indicator, Version Number and Mode values
            ntpData[0] = 0x1B; //LI = 0 (no warning), VN = 3 (IPv4 only), Mode = 3 (Client Mode)

            var addresses = Dns.GetHostEntry(ntpServer).AddressList;

            // Addresses[1] since we want to use IPv4
            // The UDP port number assigned to NTP is 123
            IPEndPoint ipEndPoint;

            try
            {
                ipEndPoint = new IPEndPoint(addresses[1], 123);
            }
            catch (Exception e)
            {
                ipEndPoint = new IPEndPoint(addresses[0], 123);
            }
            // NTP uses UDP

            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                socket.Connect(ipEndPoint);

                //Stops code hang if NTP is blocked
                socket.ReceiveTimeout = 30000;

                socket.Send(ntpData);
                socket.Receive(ntpData);
                socket.Close();
            }

            // Offset to get to the "Transmit Timestamp" field (time at which the reply 
            // departed the server for the client) in 64-bit timestamp format."
            const byte serverReplyTime = 40;

            // Get the seconds part
            ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);

            // Get the seconds fraction
            ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);

            // Convert From big-endian to little-endian
            intPart = SwapEndianness(intPart);
            fractPart = SwapEndianness(fractPart);

            var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);

            // Set the time difference
            return DateTime.Now - (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds((long)milliseconds);
        }

        // stackoverflow.com/a/3294698/162671
        private uint SwapEndianness(ulong x)
        {
            return (uint)(((x & 0x000000ff) << 24) +
                           ((x & 0x0000ff00) << 8) +
                           ((x & 0x00ff0000) >> 8) +
                           ((x & 0xff000000) >> 24));
        }

        public DateTime GetISTime()
        {
            return DateTime.UtcNow + TimeDifference;
        }


    }
}
