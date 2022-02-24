using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using libdebug;
using CEServerPS4.PS4API;
using System.Windows.Forms;

namespace CEServerPS4
{
    class Program
    {

        static int pid;
        static Thread t;
        static uint tid=0;

        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PS4CEServerWindows());
            //if (!(args.Length > 0))
            //{
            //    Console.WriteLine("use CEServerPS4 <ip>");
            //    Console.WriteLine("Example CEServerPS4 197.168.137.2");
            //    return;
            //}

            //Start(args[0]);

        }

       
        public static void Start(string ip)
        {
            CheatEngineConstants.isCustomCheatEngine = false;
            CheatEngineServer cheatEngine = new CheatEngineServer(ip);
            cheatEngine.StartAsync().Wait();
        }

        //    public static void Start(string ip)
        //{
        //      CheatEngineServer cheatEngine = new CheatEngineServer(ip);
        //    PS4DBG ps4 = new PS4DBG(ip);
        //    //ps4.Connect();

        //    ProcessList list = PS4APIWrapper.GetProcessList();
        //    //cheatEngine.StartAsync().Wait();

        //    //cheatEngine.StartAsync().Wait();
        //    foreach (Process p in list.processes)
        //    {
        //        if (p.name.Contains("eboot"))
        //        {
        //            pid = p.pid;
        //            PS4APIWrapper.ProcessID = pid;
        //            break;
        //        }
        //    }

        //    Thread.Sleep(100);

        //    //while (true)
        //    //{
        //        try
        //        {
        //        //ps4.AttachDebugger(pid,DebuggerInterruptCallback);
        //       // ps4.ProcessResume();
        //        DebugAPI.StartDebug((IntPtr)0);
        //        ps4 = PS4DedugAPIWrapper.getps4();
        //        //uint tid = PS4APIWrapper.GetThreadsList()[0];
        //        //Console.WriteLine(tid);
        //        //DebugAPI.GetThreadContext((IntPtr)0, tid, out PS4API.DebugAPI.CONTEXT Context, 1);
        //        //ulong add = Context.regs.rip;
        //        //Console.WriteLine("Address:" + add);
        //        // DebugAPI.SetBreakpoint((IntPtr)0, -1, (IntPtr)0, 2150544583, 0, 1);2437280967
        //        //DebugAPI.SetBreakpoint((IntPtr)0, -1, (IntPtr)0, 2150544583, 0, 1);
        //        ps4.ChangeBreakpoint(0, true, 2150544583);
        //       // ps4.ChangeWatchpoint(0, true, PS4DBG.WATCHPT_LENGTH.DBREG_DR7_LEN_1, PS4DBG.WATCHPT_BREAKTYPE.DBREG_DR7_WRONLY, 13018470472);
        //      //  ps4.ChangeWatchpoint(0, true, PS4DBG.WATCHPT_LENGTH.DBREG_DR7_LEN_1, PS4DBG.WATCHPT_BREAKTYPE.DBREG_DR7_WRONLY, 13018470472);
        //        object ev;
        //        DebugAPI.WaitForDebugEvent((IntPtr)0, 0, out ev);
        //        DebugAPI.WaitForDebugEvent((IntPtr)0, 0, out ev);
        //        //ps4.ChangeBreakpoint(0, true, 4375540935);
        //        Console.WriteLine("break");
        //        tid = DebugAPI.getThd();
        //        //while (tid == 0)
        //        //{
        //        //    Console.WriteLine("wait");
        //        //}
        //        //    tid = DebugAPI.getThd();
        //        for (int i = 0; i < 4; i++)
        //        {
        //            //  DebugAPI.ContinueForDebugEvent((IntPtr)0, tid, 2);
        //            //  PS4DedugAPIWrapper.ContinueBreakpointThread(0, true);
        //           // PS4DedugAPIWrapper.getps4().SingleStep();
        //              ps4.SingleStep();
        //        }
        //        ps4.ProcessResume();
        //        // DebugAPI.ContinueForDebugEvent((IntPtr)0, tid, 0);
        //        // PS4DedugAPIWrapper.ContinueBreakpointThread(0, false);
        //        // DebugAPI.RemoveBreakpoint((IntPtr)0, -1, (IntPtr)0, 0);
        //       // ps4.ChangeWatchpoint(0, false, PS4DBG.WATCHPT_LENGTH.DBREG_DR7_LEN_1, PS4DBG.WATCHPT_BREAKTYPE.DBREG_DR7_WRONLY, 13018470472);
        //        // DebugAPI.ContinueForDebugEvent((IntPtr)0, tid, 0);
        //        //PS4DedugAPIWrapper.ContinueBreakpointThread(0, false);
        //        Thread.Sleep(10);
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine("error:" + e.Message);
        //        //break;
        //    }
        //    finally
        //    {
        //        PS4DedugAPIWrapper.dettachDebugger();
        //        //ps4.DetachDebugger();
        //        //ps4.Disconnect();  
        //    }
        //    //}
        //}


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

        //public static void DebuggerInterruptCallback(
        //   uint lwpid,
        //   uint status,
        //   string tdname,
        //   regs regs,
        //   fpregs fpregs,
        //   dbregs dbregs)
        //{

        //    tid = lwpid;
        //    Console.WriteLine("address=:" + regs.r_rip);
        //    Console.WriteLine("thread=:" + lwpid);
        //    Console.WriteLine("breakaddress=:" + dbregs.dr0);

        //}

    }
}
