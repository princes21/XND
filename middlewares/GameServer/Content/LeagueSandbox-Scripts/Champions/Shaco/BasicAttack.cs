using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using System.Numerics;

namespace Spells
{
    public class ShacoBasicAttack : ISpellScript
    {
        private ObjAIBase owner;
        private static Random rng = new();

        public SpellScriptMetadata ScriptMetadata { get; private set; } = new()
        {
            MissileParameters = new MissileParameters()
        };

        public void OnActivate(ObjAIBase owner, Spell spell) =>
            this.owner = spell.CastInfo.Owner;

        private bool IsBehind(ObjAIBase attacker, AttackableUnit target)
        {
            var attackerPos = attacker.Position;
            var targetPos = target.Position;
            // crude back-stab check
            return attackerPos.Y < targetPos.Y;
        }

        public void OnLaunchAttack(AttackableUnit targetUnit, bool isCrit)
        {
            if (targetUnit is not Champion) return;

            var isDeceive = owner.HasBuff("Deceive");
            var isBackstab = !isDeceive && IsBehind(owner, targetUnit as ObjAIBase);

            var baseDamage = owner.Stats.AttackDamage.Total;

            if (isDeceive || isBackstab)
            {
                var damage = baseDamage * 2.25f;   // 200 % + 25 % bonus
                targetUnit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL,
                                    DamageSource.DAMAGE_SOURCE_ATTACK, true);
                if (isDeceive) owner.RemoveBuffsWithName("Deceive");
            }
            else
            {
                targetUnit.TakeDamage(owner, baseDamage, DamageType.DAMAGE_TYPE_PHYSICAL,
                                    DamageSource.DAMAGE_SOURCE_ATTACK, false);
            }
        }

        // stubs
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end) { }
        public void OnSpellCast(Spell spell) { }
        public void OnSpellPostCast(Spell spell) { }
        public void OnSpellChannel(Spell spell) { }
        public void OnSpellChannelCancel(Spell spell) { }
        public void OnSpellPostChannel(Spell spell) { }
        public void OnUpdate(float diff) { }
        public void OnDeactivate(ObjAIBase owner, Spell spell) { }
    }
}