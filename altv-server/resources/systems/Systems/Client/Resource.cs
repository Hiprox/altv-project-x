using AltV.Net.Client;
using AltV.Net.Client.Async;
using AltV.Net.Client.Elements.Entities;

public class Client : AsyncResource
{
    
    public override void OnStart()
    {
        Alt.Log("[Systems] Client запускается...");
        
        Alt.Log("[Systems] Client запустился!");
    }

    public override void OnStop()
    {
        
    }
}
