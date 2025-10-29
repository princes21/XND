using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerCore.Scripting.CSharp;

namespace ItemSpells
{
    public class ZhonyasHourglass : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnSpellCast(Spell spell)
        {
			if (spell.CastInfo.Owner is Champion c)
            {
				 AddBuff("ZhonyasHourglass", 2.5f, 1, spell, c, c,false);     
            }
        }
    }
}