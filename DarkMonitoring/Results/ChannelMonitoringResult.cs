using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkMonitoring.Results
{
    public struct ChannelMonitoringResult
    {
        public long SendPackets { get; set; }
        public long ReceivePackets { get; set; }
        public long SendBytes { get; set; }
        public long ReceiveBytes { get; set; }
        public DateTime Timestamp { get; set; }
        public ChannelMonitoringResult(long SendPackets, long ReceivePackets, long SendBytes, long ReceiveBytes)
        {
            this.SendPackets = SendPackets;
            this.ReceivePackets = ReceivePackets;
            this.SendBytes = SendBytes;
            this.ReceiveBytes = ReceiveBytes;
            this.Timestamp = DateTime.Now;
        }
    }
}
