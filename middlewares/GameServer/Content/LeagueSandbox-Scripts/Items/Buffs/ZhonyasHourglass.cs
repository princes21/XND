using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects;
using GameServerCore.Scripting.CSharp;

namespace Buffs
{
    class ZhonyasHourglass : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Particle Gold;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
			if (ownerSpell.CastInfo.Owner is Champion c)
            {
				 c.PauseAnimation(true);
				 c.StopMovement(); 
                 c.SetTargetUnit(null, true);	
				 SetStatus(c, StatusFlags.Stunned, true);
                 SetStatus(c, StatusFlags.Invulnerable, true);
				 c.Stats.SetActionState(ActionState.CAN_MOVE, false);
				 c.Stats.SetActionState(ActionState.CAN_ATTACK, false);
                 c.Stats.SetActionState(ActionState.CAN_CAST, false);
				 buff.SetStatusEffect(StatusFlags.Targetable, false);
				 Gold = AddParticleTarget(c, c, "zhonyas_ring", c, buff.Duration);
				 Gold = AddParticleTarget(c, c, "zhonyas_cylinder", c, buff.Duration);
				 Gold = AddParticleTarget(c, c, "zhonya_ring_self_skin", c, buff.Duration);
				 Gold = AddParticleTarget(c, c, "zhonyas_ring_activate", c, buff.Duration);
            }
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
			RemoveParticle(Gold);
			if (ownerSpell.CastInfo.Owner is Champion c)
            {
				 c.PauseAnimation(false);
				 SetStatus(c, StatusFlags.Stunned, false);
                 SetStatus(c, StatusFlags.Invulnerable, false);
				 c.Stats.SetActionState(ActionState.CAN_MOVE, true);
				 c.Stats.SetActionState(ActionState.CAN_ATTACK, true);
                 c.Stats.SetActionState(ActionState.CAN_CAST, true);
				 buff.SetStatusEffect(StatusFlags.Targetable, true);
            }
        }

        public void OnUpdate(float diff)
        {

        }
    }
}
