using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;

namespace Buffs
{
    class GuardianAngelCooldown : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_DEHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
    }
}

namespace Buffs
{
    class GuardianAngel : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_DEHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
    }
}

namespace Buffs
{
    class HasBeenRevived : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_DEHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
    }
}