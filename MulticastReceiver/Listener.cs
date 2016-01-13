/*
 * In his exalted name
 * This class starts listening on the specified multicast group ip address/port.
 * Written by Ahmad Siavashi (ahmad.siavashi@gmail.com)
 * 13/1/2016
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace MulticastReceiver
{
    class Listener
    {
        private const int CAST_COUNT = 3;
        private const int CAST_DELAY = 5000; // 1000ms = 1sec
        private const int ANY_AVAILABLE_PORT = 0;

        private IPEndPoint multicastGroup;
        private List<UdpClient> hostEntries = new List<UdpClient>();

        public Listener(IPAddress multicastIP, int multicastPort)
        {
            this.multicastGroup = new IPEndPoint(multicastIP, multicastPort);
            this.retrieveHostEntries();
        }

        public void retrieveHostEntries()
        {
            string hostName = Dns.GetHostName();
            foreach (IPAddress interfaceIP in (from ip in Dns.GetHostEntry(hostName).AddressList where ip.AddressFamily == AddressFamily.InterNetwork select ip))
            {
                UdpClient client = new UdpClient();
                client.AllowNatTraversal(true);
                client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                client.Client.Bind(new IPEndPoint(interfaceIP, multicastGroup.Port));
                client.JoinMulticastGroup(this.multicastGroup.Address);
                this.hostEntries.Add(client);
            }
        }

        IPEndPoint _RemoteEP;

        public IPEndPoint LocalEntryPoint
        {
            get { return _RemoteEP; }
            set { _RemoteEP = value; }
        }

        public void ReceiveCallBack(IAsyncResult Result)
        {
            var Args = (object[])Result.AsyncState;
            var UdpClient = (UdpClient)Args[0];
            byte[] Data = UdpClient.EndReceive(Result, ref _RemoteEP);
            string Message = Encoding.ASCII.GetString(Data);
            Console.WriteLine("{0} / {1}:{2} -> {3}", DateTime.Now.ToShortTimeString(), _RemoteEP.Address, _RemoteEP.Port, Message);
            UdpClient.BeginReceive(ReceiveCallBack, new object[] {
                     UdpClient
                    });
        }

        public void ListenLoop()
        {
            foreach (UdpClient client in this.hostEntries)
            {
                client.BeginReceive(ReceiveCallBack, new object[] {
                     client
                    });
            }

        }
    }
}
