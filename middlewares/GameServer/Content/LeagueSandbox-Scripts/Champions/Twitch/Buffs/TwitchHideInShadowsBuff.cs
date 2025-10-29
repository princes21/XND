using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    class TwitchHideInShadowsBuff : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Particle pbuff;
        Particle pbuff2;
        Buff thisBuff;
		Spell Spell;
		ObjAIBase owner;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
			Spell = ownerSpell;
            thisBuff = buff;
			owner = ownerSpell.CastInfo.Owner as Champion;
			if (unit is ObjAIBase ai)
            {
            StatsModifier.AttackSpeed.FlatBonus = 1.20f;
			unit.AddStatModifier(StatsModifier);
            ai.CancelAutoAttack(false, true);
			}
        }      
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
			RemoveParticle(pbuff);
            RemoveParticle(pbuff2);
			RemoveBuff(thisBuff);				
        }
    }
}