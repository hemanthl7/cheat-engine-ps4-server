using CEServerPS4.CheatEnginePackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using CEServerPS4.CheatEnginePackets.C2S;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace CEServerPS4
{
    public class CheatEngineServer : IDisposable
    {
        private TcpListener _tcpListener;
        private PacketManager packetManager;
        private CancellationTokenSource _tokenSource;

        private bool isDisposed
        {
            get;set;
        }
        public static bool islistening
        {
            get; set;
        }

        public static ConnectType isConnected
        {
            get; set;
        }
        private CancellationToken _token;
        

        public CheatEngineServer(string ip,ushort port = 52736) : this(port, new PacketManager())
        {
            PS4API.PS4Static.IP = ip;
            PS4API.PS4APIWrapper.Connect();
            this.RegisterDefaultHandlers();
            isConnected = ConnectType.INIT;
        }

        public CheatEngineServer(PacketManager pm) : this(52736, pm)
        {

        }

        public CheatEngineServer(ushort port, PacketManager pm)
        {
            _tcpListener = new TcpListener(IPAddress.Any, port);
            this.packetManager = pm;
        }

        private void HandleReceivedClient(TcpClient client)
        {

            var clientStream = client.GetStream();
            var reader = new BinaryReader(clientStream);
            var writer = new BinaryWriter(clientStream);
            while (true)
            {
                try
                {

                    var command = this.packetManager.ReadNextCommand(reader);
                    var output = this.packetManager.ProcessAndGetBytes(command);
                   
                    writer.Write(output);
                    writer.Flush();                   

                }
                catch(EndOfStreamException)
                {
                    client.Close();
                    break;
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e + ": "+  e.Message);
                    Trace.WriteLine(e.StackTrace);
                    client.Close();
                    Dispose();
                    break;
                }
              
            }
        }

        public async Task StartAsync(CancellationToken? token = null)
        {
            _tokenSource = CancellationTokenSource.CreateLinkedTokenSource(token ?? new CancellationToken());
            _token = _tokenSource.Token;
            try
            {
                _tcpListener.Start();
                isConnected = ConnectType.SUCCESS;
                islistening = true;
                isDisposed = false;
                while (!_token.IsCancellationRequested)
                {
                    if (islistening)
                    {
                        var tcpClientTask = _tcpListener.AcceptTcpClientAsync();
                        var result = await tcpClientTask;
                        _ = Task.Run(() =>
                        {
                            HandleReceivedClient(result);
                        }, _token);
                    }
                    
                }
            }
            finally
            {
                if (islistening)
                {
                    _tcpListener.Stop();
                    islistening = false;
                }
                if (!ConnectType.SUCCESS.Equals(isConnected))
                {
                    isConnected = ConnectType.FAILED;
                }
                Dispose();            
            }
        }

        public void Stop()
        {
            _tokenSource?.Cancel();
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                isDisposed = true;
                Stop();
                if (islistening)
                {
                    _tcpListener.Stop();
                    islistening = false;
                }
                PS4API.PS4DedugAPIWrapper.dettachDebugger();
                PS4API.PS4APIWrapper.Disconnect();
            }
            
        }

        private void RegisterDefaultHandlers()
        {
            this.RegisterCommandHandler(new CreateToolHelp32SnapshotCommand());
            this.RegisterCommandHandler(new GetVersionCommand());
            this.RegisterCommandHandler(new Module32FirstCommand());
            this.RegisterCommandHandler(new Module32NextCommand());
            this.RegisterCommandHandler(new Process32FirstCommand());
            this.RegisterCommandHandler(new Process32NextCommand());
            this.RegisterCommandHandler(new CloseHandleCommand());
            this.RegisterCommandHandler(new OpenProcessCommand());
            this.RegisterCommandHandler(new GetArchitectureCommand());
            this.RegisterCommandHandler(new VirtualQueryExCommand());
            this.RegisterCommandHandler(new VirtualQueryExFullCommand());
            this.RegisterCommandHandler(new ReadProcessMemoryCommand());
            this.RegisterCommandHandler(new GetSymbolsFromFileCommand());
            this.RegisterCommandHandler(new StartDebugCommand());
            this.RegisterCommandHandler(new ResumeThreadCommand());
            this.RegisterCommandHandler(new SuspendThreadCommand());
            this.RegisterCommandHandler(new SetBreakPointCommand());
            this.RegisterCommandHandler(new RemoveBreakPointCommand());
            this.RegisterCommandHandler(new GetThreadContextCommand());
            this.RegisterCommandHandler(new WaitForDebugEventCommand());
            this.RegisterCommandHandler(new ContinueForDebugEventCommand());
            this.RegisterCommandHandler(new WriteProcessMemoryCommand());
            this.RegisterCommandHandler(new GetABICommand());
        }

        public void RegisterCommandHandler(ICheatEngineCommand command)
        {
            this.packetManager.RegisterCommand(command);
        }

    }
}
