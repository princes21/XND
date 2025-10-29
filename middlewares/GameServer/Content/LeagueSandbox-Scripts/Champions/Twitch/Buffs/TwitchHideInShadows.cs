using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    class TwitchHideInShadows : IBuffGameScript
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
			StatsModifier.MoveSpeed.PercentBonus = 0.25f;
			unit.AddStatModifier(StatsModifier);
			pbuff = AddParticleTarget(ai, ai, "Twitch_Base_Q_Haste", ai, lifetime: buff.Duration);
			AddParticle(ai, null, "Twitch_Base_Q_Bamf", ai.Position, lifetime: buff.Duration);
            AddParticle(ai, null, "Twitch_Base_Q_Cas_Invisible", ai.Position, lifetime: buff.Duration);		
			ApiEventManager.OnLaunchAttack.AddListener(this, ai, OnLaunchAttack, false);
            ai.CancelAutoAttack(false, true);
			}
        }
        public void OnLaunchAttack(Spell spell)
        {
            RemoveBuff(owner, "Invisibility");
            RemoveBuff(owner, "Targetable");
			if (thisBuff != null && thisBuff.StackCount != 0 && !thisBuff.Elapsed())
            {
            thisBuff.DeactivateBuff();
			}
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveBuff(thisBuff);
            if (buff.TimeElapsed >= buff.Duration)
            {
                ApiEventManager.OnPreAttack.RemoveListener(this);
                ApiEventManager.OnLaunchAttack.RemoveListener(this);
            }
            if (unit is ObjAIBase ai)
            {
                AddBuff("TwitchHideInShadowsBuff", 5f, 1, Spell, ai, ai);
                AddParticle(ai, null, "Twitch_Base_Q_Invisiible_Outro", ai.Position, lifetime: buff.Duration);
            }
        }
    }
}
