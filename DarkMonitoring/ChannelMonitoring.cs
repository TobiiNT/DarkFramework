using DarkMonitoring.Delegates;
using DarkMonitoring.Results;

namespace DarkMonitoring
{
    public class ChannelMonitoring
    {
        private Thread? ThreadMonitoring { set; get; }
        public string MornitorName { get; private set; }
        private object LockObject = new object();
        private List<uint> ConccurrentConnections;
        private long TotalNewConnections;
        private long TotalConnectConnections;
        private long TotalDisconnectConnections;
        private long TotalInterruptConnections;
        private long TotalSendPackets;
        private long TotalReceivePackets;
        private long TotalSendBytes;
        private long TotalReceiveBytes;

        public ChannelMonitoring(string MornitorName)
        {
            this.MornitorName = MornitorName;
            this.ConccurrentConnections = new List<uint>();

            this.ResetMonitoring();
        }

        public void StartMonitoring()
        {
            this.ResetMonitoring();

            this.ThreadMonitoring = new Thread(new ThreadStart(Mornitoring));
            this.ThreadMonitoring.Name = this.MornitorName;
            this.ThreadMonitoring.Start();
        }

        private void ResetMonitoring()
        {
            Interlocked.Exchange(ref this.TotalNewConnections, 0);
            Interlocked.Exchange(ref this.TotalDisconnectConnections, 0);
            Interlocked.Exchange(ref this.TotalInterruptConnections, 0);
            Interlocked.Exchange(ref this.TotalSendPackets, 0);
            Interlocked.Exchange(ref this.TotalSendBytes, 0);
            Interlocked.Exchange(ref this.TotalReceivePackets, 0);
            Interlocked.Exchange(ref this.TotalReceiveBytes, 0);
        }

        public void MornitorNewConnection(uint ClientID)
        {
            lock (LockObject)
            {
                if (!this.ConccurrentConnections.Contains(ClientID))
                    this.ConccurrentConnections.Add(ClientID);
            }
           
            Interlocked.Increment(ref this.TotalNewConnections);
        }

        public void MornitorConnectConnection()
        {
            Interlocked.Increment(ref this.TotalConnectConnections);
        }

        public void MornitorDisconnectConnection(uint ClientID)
        {
            lock (LockObject)
            {
                if (this.ConccurrentConnections.Contains(ClientID))
                    this.ConccurrentConnections.Remove(ClientID);
            }
            Interlocked.Increment(ref this.TotalDisconnectConnections);
        }

        public void MornitorInterruptConnection(uint ClientID)
        {
            lock (LockObject)
            {
                if (this.ConccurrentConnections.Contains(ClientID))
                    this.ConccurrentConnections.Remove(ClientID);
            }
            Interlocked.Increment(ref this.TotalInterruptConnections);
        }

        public void MornitorSendData(int DataSize)
        {
            Interlocked.Increment(ref this.TotalSendPackets);
            Interlocked.Add(ref this.TotalSendBytes, DataSize);
        }

        public void MornitorReceiveData(int DataSize)
        {
            Interlocked.Increment(ref this.TotalReceivePackets);
            Interlocked.Add(ref this.TotalReceiveBytes, DataSize);
        }

        private void Mornitoring()
        {
            while (true)
            {
                try
                {
                    ChannelMonitoringResult Result = new ChannelMonitoringResult(
                        Interlocked.Read(ref this.TotalSendPackets),
                        Interlocked.Read(ref this.TotalReceivePackets),
                        Interlocked.Read(ref this.TotalSendBytes),
                        Interlocked.Read(ref this.TotalReceiveBytes)
                    );

                    this.ReturnNetworkMonitoringResult(Result);
                    this.ResetMonitoring();

                    Thread.Sleep(1000);
                }
                catch
                {
                    this.StartMonitoring();
                }
            }
        }

        public event ChannelMonitoringCallback? NetworkMonitoringCallback;
        public void ReturnNetworkMonitoringResult(ChannelMonitoringResult Result) => NetworkMonitoringCallback?.Invoke(Result);
    }
}