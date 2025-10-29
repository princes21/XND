using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;


namespace Buffs
{
    class DianaMoonlight : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData 
        {
            BuffType = BuffType.COMBAT_DEHANCER
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Particle p;
        Particle p2;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
			if(unit is Champion)
			{
            p = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Diana_Base_Q_Moonlight_Champ", unit, buff.Duration);
			}
			else
			{
             p = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Diana_Base_Q_Moonlight", unit, buff.Duration);
			}
            //TODO: Find the overhead particle effects
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(p);
            RemoveParticle(p2);
        }
    }
}
