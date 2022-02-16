using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using libdebug;
using CEServerPS4.PS4API;

namespace CEServerPS4
{
    class Program
    {

        static int pid;
        static Thread t;
       

        static void Main(string[] args)
        {

            if (!(args.Length > 0))
            {
                Console.WriteLine("use CEServerPS4 <ip>");
                Console.WriteLine("Example CEServerPS4 197.168.137.2");
                return;
            }

            Start(args[0]);

        }

        public static void DebuggerInterruptCallback(
           uint lwpid,
           uint status,
           string tdname,
           regs regs,
           fpregs fpregs,
           dbregs dbregs)
        {

            
            Console.WriteLine("status=:" + regs.r_rip);


        }


        //public static void rbuffer(object address)
        //{
           
        //    while (true)
        //    {
        //        try
        //        {
        //            byte[] bs = PS4APIWrapper.ReadMemory((ulong)address, 20);
        //            Console.WriteLine("String" + Encoding.UTF8.GetString(bs));
        //        }catch(Exception e)
        //        {
        //            Console.WriteLine("error:" + e.Message);
        //            break;
        //        }
                
        //    }
            
        //}

        public static void Start(string ip)
        {
            CheatEngineServer cheatEngine = new CheatEngineServer(ip);
            ProcessList list = PS4APIWrapper.GetProcessList();
            cheatEngine.StartAsync().Wait();

            //cheatEngine.StartAsync().Wait();
            //foreach (Process p in list.processes)
            //{
            //    if (p.name.Contains("eboot"))
            //    {
            //        pid = p.pid;
            //        PS4APIWrapper.ProcessID = pid;
            //        break;
            //    }
            //}

            //DebugAPI.StartDebug((IntPtr)0);

            //Thread.Sleep(100);

            //while (true)
            //{
            //    try
            //    {
            //        uint tid = PS4APIWrapper.GetThreadsList()[0];
            //        Console.WriteLine(tid);
            //        DebugAPI.GetThreadContext((IntPtr)0, tid, out PS4API.DebugAPI.CONTEXT Context, 1);
            //        ulong add = Context.regs.rip;
            //        Console.WriteLine("Address:"+add);
            //        DebugAPI.SetBreakpoint((IntPtr)0,0, (IntPtr)0, 1663069276,  4, 3);
            //        Console.WriteLine("break");
            //        //DebugAPI.RemoveBreakpoint((IntPtr)0, 0, (IntPtr)0, 0);
            //        Thread.Sleep(10);
            //        runSecondThread(add);
            //    }catch(Exception e)
            //    {
            //        Console.WriteLine("error:" + e.Message);
            //        break;
            //    }
            //}
        }

    }
}
