using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Interactions;
using System.Numerics;
using AltV.Net.Async;
using System.Globalization;

/*public class Interactions : IScript
{
    [ClientEvent("Server:Interaction:Trigger")]
    public static void OnInteractionTrigger(CPlayer player, ulong type)
    {
        Server.Interactions.Trigger(type, player);
    }
}

public class TestInteraction : Interaction
{
    public int Value { get; }
    public TestInteraction(int value, ulong type, ulong id, Vector3 position, int dimension, uint range) : base(type, id,
            position, dimension, range)
    {
        Value = value;
    }
    public override bool OnInteraction(IPlayer player, Vector3 interactionPosition, int interactionDimension, object argument)
    {
        Alt.Log($"OnInteraction Triggered: {Value}");
        return false;
    }
}*/
namespace AltServer
{
    public class Server : AsyncResource
    {
        public Server() : base(true) { }
        //private static InteractionsServiceBuilder _Builder { get; set; }
        // public static InteractionsService Interactions { get; set; }
        public override void OnStart()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.DefaultThreadCurrentCulture ?? CultureInfo.InvariantCulture;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[START] Resource 'systems'");

            AltDataBase.Instance();
            AltDataBase.Connect();
            InteriorsManager.Instance();

            /*_Builder = InteractionsService.CreateBuilder();
            ulong type = 1;
            _Builder.AddThreadForType(type);
            Interactions = _Builder.Build();
            var item = new TestInteraction(1337, type, 0, new System.Numerics.Vector3(0f, 0f, 75f), 0, 5);
            Interactions.Add(item);*/
        }
        public override void OnStop()
        {
            AltDataBase.Close();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[STOP] Resource 'systems'");
        }

        public override IEntityFactory<IPlayer> GetPlayerFactory()
        {
            return new CPlayerFactory();
        }
    }
}