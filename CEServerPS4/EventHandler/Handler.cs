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
        public CommandType CommandType => CommandType.CMD_SETWATCHPOINT;

        public void handle(Object obj, out Object response)
        {
            response = 0;
            SetWatchPointEvent watchPointEvent = (SetWatchPointEvent)obj;
            SetWatchPointRequest r = (SetWatchPointRequest)watchPointEvent.Data;
            PS4DedugAPIWrapper.SetWatchpoint(r.Tid, r.Address, r.Bpsize, r.Bptype);
            response = 1;
        }
    }


    public class RemoveWatchPointHandler : IHandler
    {
        public CommandType CommandType => CommandType.CMD_REMOVEWATCHPOINT;

        public void handle(Object obj, out Object response)
        {
            RemoveWatchPointEvent watchPointEvent = (RemoveWatchPointEvent)obj;
            RemoveWatchPointRequest r = (RemoveWatchPointRequest)watchPointEvent.Data;
            response = 0; 

            if (r.Tid == -1)
            {
                PS4DedugAPIWrapper.ClearWatchPoints();
                response = 1;
            }
            else
            {
                PS4DedugAPIWrapper.ClearWatchpoint(r.Tid);
                response = 1;
            }
        }
    }

    public class SetBreakPointHandler : IHandler
    {
        public CommandType CommandType => CommandType.CMD_SETBREAKPOINT;

        public void handle(Object obj, out Object response)
        {
            response = 0;
            SetBreakPointEvent breakPointEvent = (SetBreakPointEvent)obj;
            SetBreakPointRequest r = (SetBreakPointRequest)breakPointEvent.Data;
            PS4DedugAPIWrapper.SetBreakpoint(r.Tid, r.Address);
            response = 1;
        }
    }


    public class RemoveBreakPointHandler : IHandler
    {
        public CommandType CommandType => CommandType.CMD_REMOVEBREAKPOINT;

        public void handle(Object obj, out Object response)
        {
            RemoveBreakPointEvent breakPointEvent = (RemoveBreakPointEvent)obj;
            RemoveBreakPointRequest r = (RemoveBreakPointRequest)breakPointEvent.Data;
            response = 0;

            if (r.Tid == -1)
            {
                PS4DedugAPIWrapper.ClearBreakpoints();
                response = 1;
            }
            else
            {
                PS4DedugAPIWrapper.ClearBreakpoint(r.Tid);
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

            PS4DedugAPIWrapper.StopDebuggerThread(r.Tid);
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

            PS4DedugAPIWrapper.resumeDebuggerThread(r.Tid);
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
            response = PS4DedugAPIWrapper.getRegisters(r.Tid);

        }
    }


    public class ContinueDebugEventHandler : IHandler
    {
        public CommandType CommandType => CommandType.CMD_CONTINUEFROMDEBUGEVENT;

        public void handle(Object obj, out Object response)
        {
            ContinueDebugEvent Cevent = (ContinueDebugEvent)obj;
            ContinueDebugEventRequest rq = (ContinueDebugEventRequest)Cevent.Data;
            response = 0;
            PS4DedugAPIWrapper.ContinueBreakpointThread(rq.Tid, rq.singleStep);
            response = 1;

        }
    }
}
