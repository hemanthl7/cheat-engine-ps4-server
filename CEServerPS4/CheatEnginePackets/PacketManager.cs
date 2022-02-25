using CEServerPS4.CheatEnginePackets.C2S;
using CEServerPS4.CheatEnginePackets.S2C;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace CEServerPS4.CheatEnginePackets
{
    /**
     * This classes parses incoming commands
     * It's possible to register commands to parse with the RegisterCommand method
     * 
     * The instance passed is utilized to write the data to, no copying is made! 
     * This is because Cheat Engine always waits for a response to the requested command so parellism/async is handling is not necessary.
     * To solve this you could get the type of the passed command and instanciate a new one through reflection but some performance would be lost with this.
     * Because of this one server can only server one client at a time, maybe on a future release add this support?
     */
    public class PacketManager
    {

        private Dictionary<CommandType, ICheatEngineCommand> commandHandler = new Dictionary<CommandType, ICheatEngineCommand>();


        public void RegisterCommand(ICheatEngineCommand command)
        {
            if (commandHandler.ContainsKey(command.CommandType))
                commandHandler.Remove(command.CommandType);
            commandHandler.Add(command.CommandType, command);
        }

        public ICheatEngineCommand ReadNextCommand(BinaryReader reader)
        {
            CommandType type = (CommandType)reader.ReadByte();

            ICheatEngineCommand command;
            if(!commandHandler.TryGetValue(type, out command))
            {
                //Unoptimal solution if multiple commands can be sent at once
                //But if that were the case knowledge of all commands would be necessary
                //As the protocol doesn't provide a way to know the 'size' of the incoming message
                //this.consumeAll(reader);
                Trace.WriteLine(type);
                throw new MissingCommandHandlerException();
            }
            command = (ICheatEngineCommand)Activator.CreateInstance(command.GetType()); 
            command.Initialize(reader);

            return command;
            
        }

        public byte[] ProcessAndGetBytes(ICheatEngineCommand command)
        {
            if(!command.initialized)
            {
                throw new C2S.Exceptions.CommandNotInitializedException();
            }
            var ret = command.ProcessAndGetBytes();
            command.Unintialize();
            return ret;
        }

        private byte[] consumeAll(BinaryReader reader)
        {
            byte[] all;

            byte[] data = new byte[1024];
            using (MemoryStream ms = new MemoryStream())
            {

                int numBytesRead;
                while ((numBytesRead = reader.Read(data, 0, data.Length)) > 0)
                {
                    ms.Write(data, 0, numBytesRead);
                    if (numBytesRead < data.Length)
                        break;

                }
                all = ms.ToArray();
            }
            return all;
        }
    }
}
