using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using libdebug;
using System.Threading.Tasks.Dataflow;
using System.Threading.Tasks;
using CEServerPS4.EventHandler.Event;
using CEServerPS4.CheatEnginePackets;

namespace CEServerPS4.EventHandler
{
    public static class DebugEventHandler
    {

        private static BlockingCollection<object> debugEvents = new BlockingCollection<Object>();
        private static Dictionary<CommandType,IHandler> handlers = new Dictionary<CommandType, IHandler>();


        static DebugEventHandler()
        {
            new Thread(listen).Start();
            RegisterHandlers();
        }

       public static void Produce(Object block,Object data)
        {

            ITargetBlock<Object> target = (ITargetBlock<Object>)block;
            target.Post(data);

            target.Complete();
        }

        public static async Task<object> ConsumeAsync(object obj) 
        {

            ISourceBlock<object> source = (ISourceBlock<object>)obj;

            while (await source.OutputAvailableAsync())
            {
                return await source.ReceiveAsync();
                
            }

            return null;
        }

        public static void AddEvent(Object debugThreadEvent)
        {
            debugEvents.Add(debugThreadEvent);
        }

        public static void listen()
        {
            while (true)
            {
                try
                {
                    object obj = debugEvents.Take();
                    Handle(obj);
                }
                catch (Exception)
                {
                    Console.WriteLine("Error Executing Event");
                }
            }
        }

        public static void Handle(object DebuggerEvent)
        {

            DebugThreadEvent debugThreadEvent = (DebugThreadEvent)DebuggerEvent;
            Object response = null;
            try
            {
                if (!handlers.TryGetValue(debugThreadEvent.CommandType, out IHandler handler))
                {

                    Console.WriteLine("handler not found:" + debugThreadEvent.CommandType);
                    throw new MissingCommandHandlerException();
                }
                handler.handle(debugThreadEvent ,out response);
            }
            catch (Exception)
            {
                Console.WriteLine("Exception ocuccerd in " + debugThreadEvent.CommandType);
            }
            finally
            {
                DebugEventHandler.Produce(debugThreadEvent.BufferBlock, response);
            }

            
        }

        public static void RegisterHandler(IHandler handler)
        {
            if (handlers.ContainsKey(handler.CommandType))
                handlers.Remove(handler.CommandType);
            handlers.Add(handler.CommandType, handler);
        }

        public static void RegisterHandlers()
        {
            RegisterHandler(new SetWatchPointHandler());
            RegisterHandler(new ResumeThreadHandler());
            RegisterHandler(new SuspendThreadHandler());
            RegisterHandler(new RemoveWatchPointHandler());
            RegisterHandler(new ThreadContextHandler());
            RegisterHandler(new ProcessRumeHandler());
        } 

    }

}