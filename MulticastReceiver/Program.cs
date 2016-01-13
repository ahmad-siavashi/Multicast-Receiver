using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace MulticastReceiver
{
    class Program
    {
        private static void usage(){
            Console.WriteLine("[-] Help: MulticastReceiver <multicast_group_ip> <port>");
        }

        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 2)
                {
                    usage();
                    return;
                }
                Console.WriteLine("[O] Multicast Message Receiver");
                Console.WriteLine("[O] A simple program to receive multicast messages in form of ASCII strings.");
                Console.WriteLine("[O] Written by Ahmad Siavashi (ahmad.siavashi@gmail.com)");
                Console.WriteLine("[O] Winter 2016");
                IPAddress group_ip_address = IPAddress.Parse(args[0]);
                int port = int.Parse(args[1]);
                Console.WriteLine("[+] Initializing the listener...");
                Listener listener = new Listener(group_ip_address, port);
                Console.WriteLine("[X] Press any key to exit.");
                Console.WriteLine("[-] Listening on: {0}:{1}", args[0], args[1]);
                Console.WriteLine("[-] Receiving...");
                listener.ListenLoop();
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[-] Error:" + ex.Message);
            }
        }
    }
}
