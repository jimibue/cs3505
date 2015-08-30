using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BB;
using BoggleServer;
using System.Threading;
using BoggleClient;

namespace Launcher
{
    public class Program
    {
        static void Main(string[] args)
        {
            new Thread( () => BoggleServer.BoggleServer.Main(args)).Start(); 

            // TA James gave us this. 
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = @"..\..\..\BoggleClient\bin\Debug\BoggleClient";
            new Thread(() => p.Start()).Start();

            System.Diagnostics.Process p1 = new System.Diagnostics.Process();
            p1.StartInfo.FileName = @"..\..\..\BoggleClient\bin\Debug\BoggleClient";
            new Thread(() => p1.Start()).Start();
        }
    }
}
