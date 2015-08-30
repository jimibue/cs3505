using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BB;
using BoggleServer;
using System.Threading;

namespace BoggleClientView
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Create a boggle server and clients.
            new BoggleServer.BoggleServer(20, "..\\..\\..\\Resources\\Libraries\\dictionary.txt", "LAAYVIXZEOHOSMEN");
            new Thread(() => BoggleClientView.Main()).Start();
            new Thread(() => BoggleClientView.Main()).Start();
            //new Thread(() => ChatClientView.Main()).Start();
            //new Thread(() => ChatClientView.Main()).Start();
        }
    }
}
