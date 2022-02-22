
# Cheat engine windows server
First of All,
A Big Thanks to CTN for Sharing Libdebug soucre code without it would be possible.
I am Gratful to these Devlpoers and Testers:
- [@CTN](https://github.com/ctn123)
- [@Shiningami](https://ko-fi.com/shiningami)

Testers:
- Rizla 
- [@GrimDoe](https://twitter.com/GrimDoe)

A port of the linux/android cheat engine server to Windows utilizing C# ported from the [official Cheat Engine repositotry](https://github.com/cheat-engine/cheat-engine/tree/a2d035583c35c0cb2455bd9aef771efbba1570c3/Cheat%20Engine/ceserver).
The initial  reason this project was created was to 'bypass' some applications that block Cheat Engine when running in the same machine or just refuse to run when Cheat Engine is running as well. With this you can run Cheat Engine in a separate environment and connect to the target machine.

# Example usage
If you just want to run the server as is there is a console project named **CEServerApplicaiton** that takes advantage of the generated library which is an assembly with the following code:
```csharp
            CheatEngineServer server = new CheatEngineServer();
            server.StartAsync().Wait();
```
If you just want to run the server as is there is a console application named CEServerPS4.exe:
```cmd
            CEServerPS4.exe 192.168.137.2
```

If you wish to handle a specific command from cheat engine differently or register a new one you can do this by either extending one of the defined Commands in **CEServerWindows.CheatEnginePackets.S2C** or by implementing  the **ICheatEngineResponse** interface although it is recommended to extend the base class **CheatEngineCommand**

For example you could override the **Process** method of **ReadProcessorMemoryCommand** to utilize a different way of reading the memory of the target process such as communication with a kernel module/driver.

# What doesn't work
Even though the official cheat engine server 'ports' a few of Windows API calls some code is specific to linux and are not easily brought back to windows 
* **Compression**: The current implementation of the command **ReadProcessMemory** does not implement compression, this is disabled by default in Cheat Engine (Network >> Compression)
* **Debug symbols**: Symbols are not loaded as we believe that cheat engine expects debug symbols for elf binaries
* **Speed hack**: Not implemented
* **Alloc/Free**: Not implemented
* **Aob injection**:Not Implemented 
