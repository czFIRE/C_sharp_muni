using GuerrillaNtp;
using System.Net;

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
            using (var ntp = new NtpClient(Dns.GetHostAddresses("time.fi.muni.cz")[0]))
            {
                // Time is probably broken again
                return ntp.GetCorrectionOffset() + new TimeSpan(2, 0, 0);
            }
        }

        public DateTime GetISTime()
        {
            return DateTime.UtcNow + TimeDifference;
        }


    }
}
