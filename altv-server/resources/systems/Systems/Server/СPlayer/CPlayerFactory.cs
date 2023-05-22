using AltV.Net;
using AltV.Net.Elements.Entities;

internal class CPlayerFactory : IEntityFactory<IPlayer>
{
    public IPlayer Create(ICore core, IntPtr entityPointer, ushort id)
    {
        return new CPlayer(core, entityPointer, id);
    }
}

