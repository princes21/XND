using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;


namespace Buffs
{
    internal class NasusQStacks : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COUNTER,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 999999
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
    }
}
