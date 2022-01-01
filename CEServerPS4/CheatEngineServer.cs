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

namespace CEServerPS4
{
    public class CheatEngineServer : IDisposable
    {
        private TcpListener _tcpListener;
        private PacketManager packetManager;
        private CancellationTokenSource _tokenSource;
        private bool _listening;
        private CancellationToken _token;
        private PS4API.PS4APIWrapper ps4 = PS4API.PS4APIWrapper.Instance();


        public CheatEngineServer(ushort port = 52736) : this(port, new PacketManager())
        { 
            this.RegisterDefaultHandlers();
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
                    /* if(command.CommandType != CommandType.CMD_READPROCESSMEMORY)
                         Console.WriteLine(BitConverter.ToString(output).Replace("-", ""));*/
                   // Console.WriteLine("{0} returned {1} bytes", command.CommandType, output.Length);
                    writer.Write(output);
                    writer.Flush();
                    //   Handle(stream, writer, cmd);

                }
                catch(EndOfStreamException)
                {
                    client.Close();
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e + ": "+  e.Message);
                    Console.WriteLine(e.StackTrace);
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
            _tcpListener.Start();
            _listening = true;

            try
            {
                PS4API.PS4APIWrapper.Connect("192.168.137.2");
                while (!_token.IsCancellationRequested)
                {
                    var tcpClientTask = _tcpListener.AcceptTcpClientAsync();
                    var result = await tcpClientTask;
                    Console.WriteLine("new Ps4 request");
                    _ = Task.Run(() =>
                      {
                          HandleReceivedClient(result);
                      }, _token);
                }
            }
            finally
            {
                _tcpListener.Stop();
                _listening = false;
                Dispose();            
            }
        }

        public void Stop()
        {
            _tokenSource?.Cancel();
        }

        public void Dispose()
        {
            PS4API.PS4APIWrapper.Disconnect();
            Stop();
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
        }

        public void RegisterCommandHandler(ICheatEngineCommand command)
        {
            this.packetManager.RegisterCommand(command);
        }

    }
}
