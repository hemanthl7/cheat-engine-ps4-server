using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CEServerPS4.EventHandler.Event;
using CEServerPS4.EventHandler.Request;
using CEServerPS4.PS4API;
using libdebug;

namespace CEServerPS4.EventHandler
{
    public interface IHandler
    {
        CommandType CommandType { get; }
        void handle(Object obj,out Object response);
    }

    public class SetWatchPointHandler : IHandler
    {
        public CommandType CommandType => CommandType.CMD_SETBREAKPOINT;

        public void handle(Object obj, out Object response)
        {
            response = 0;
            SetWatchPointEvent watchPointEvent = (SetWatchPointEvent)obj;
            SetWatchPointRequest r = (SetWatchPointRequest)watchPointEvent.Data;
            PS4APIWrapper.SetWatchpoint(r.Tid, r.Address, r.Bpsize, r.Bptype);
            response = 1;
        }
    }


    public class RemoveWatchPointHandler : IHandler
    {
        public CommandType CommandType => CommandType.CMD_REMOVEBREAKPOINT;

        public void handle(Object obj, out Object response)
        {
            RemoveWatchPointEvent watchPointEvent = (RemoveWatchPointEvent)obj;
            RemoveWatchPointRequest r = (RemoveWatchPointRequest)watchPointEvent.Data;
            response = 0; ;

            if (r.Tid == -1)
            {
                PS4APIWrapper.ClearWatchPoints();
                response = 1;
            }
            else
            {
                PS4APIWrapper.ClearWatchpoint(r.Tid);
                response = 1;
            }
           
           
        }
    }

    public class SuspendThreadHandler : IHandler
    {
        public CommandType CommandType => CommandType.CMD_SUSPENDTHREAD;

        public void handle(Object obj, out Object response)
        {
            SuspendThreadEvent watchPointEvent = (SuspendThreadEvent)obj;
            SuspendThreadRequest r = (SuspendThreadRequest)watchPointEvent.Data;
            response = 0;

            PS4APIWrapper.StopDebuggerThread(r.Tid);
            response=1;

        }
    }


    public class ResumeThreadHandler : IHandler
    {
        public CommandType CommandType => CommandType.CMD_RESUMETHREAD;

        public void handle(Object obj, out Object response)
        {
            ResumeThreadEvent watchPointEvent = (ResumeThreadEvent)obj;
            ResumeThreadRequest r = (ResumeThreadRequest)watchPointEvent.Data;
            response = 0;

            PS4APIWrapper.resumeDebuggerThread(r.Tid);
            response = 1;

        }
    }

    public class ThreadContextHandler : IHandler
    {
        public CommandType CommandType => CommandType.CMD_GETTHREADCONTEXT;

        public void handle(Object obj, out Object response)
        {
            ThreadContextEvent watchPointEvent = (ThreadContextEvent)obj;
            ThreadContextRequest r = (ThreadContextRequest)watchPointEvent.Data;
            response = new regs();
            response = PS4APIWrapper.getRegisters(r.Tid);

        }
    }


    public class ProcessRumeHandler : IHandler
    {
        public CommandType CommandType => CommandType.CMD_CONTINUEFROMDEBUGEVENT;

        public void handle(Object obj, out Object response)
        {
            ProcessReumeEvent watchPointEvent = (ProcessReumeEvent)obj;
            response = 0;
            PS4APIWrapper.ProcessResume();
            response = 1;

        }
    }
}
