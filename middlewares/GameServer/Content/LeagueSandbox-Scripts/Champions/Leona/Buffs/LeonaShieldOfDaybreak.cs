using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    class LeonaShieldOfDaybreak : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Particle pbuff;
        Buff thisBuff;
        Spell Spell;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;
            pbuff = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Leona_ShieldOfDaybreak_cas", unit, buff.Duration, bone: "BUFFBONE_CSTM_SHIELD_TOP");
            var owner = ownerSpell.CastInfo.Owner;

            owner.CancelAutoAttack(true);

            ApiEventManager.OnPreAttack.AddListener(owner, owner, OnPreAttack, true);
            ApiEventManager.OnLaunchAttack.AddListener(owner, owner, TargetExecute, true);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (buff.TimeElapsed >= buff.Duration)
            {
                ApiEventManager.OnPreAttack.RemoveListener(this, unit as ObjAIBase);
            }

            RemoveParticle(pbuff);
        }

        AttackableUnit target;

        public void OnPreAttack(Spell spell)
        {
            spell.CastInfo.Owner.SkipNextAutoAttack();

            SpellCast(spell.CastInfo.Owner, 0, SpellSlotType.ExtraSlots, false, spell.CastInfo.Owner.TargetUnit, Vector2.Zero);
            var owner = spell.CastInfo.Owner;


            if (thisBuff != null)
            {
                thisBuff.DeactivateBuff();
                RemoveBuff(owner, "LeonaShieldOfDaybreakMS");
            }
        }

        public void TargetExecute(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var target = spell.CastInfo.Targets[0].Unit;

            float ArMrDamage = owner.Stats.Armor.Total * 0.60f + owner.Stats.MagicResist.Total * 0.40f;
            float damage = 70 + ArMrDamage;

            if (target != null && !target.IsDead)
            {
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddBuff("Stun", 2.5f, 1, spell, target, owner);
                RemoveBuff(owner, "LeonaShieldOfDaybreakMS");
                AddParticleTarget(owner, target, "Leona_ShieldOfDaybreak_tar.troy", target, 1f);
            }
        }
    }
}
