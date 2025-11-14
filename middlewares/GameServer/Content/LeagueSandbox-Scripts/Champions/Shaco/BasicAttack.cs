using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using System;
using System.Numerics;

namespace Spells
{
    public class ShacoBasicAttack : ISpellScript
    {
        private ObjAIBase owner;
        private static Random rng = new Random();

        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters()
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            this.owner = spell.CastInfo.Owner;
        }

        private bool IsBehind(ObjAIBase attacker, AttackableUnit target)
        {
            Vector2 attackerPos = new Vector2(attacker.Position.X, attacker.Position.Y);
            Vector2 targetPos = new Vector2(target.Position.X, target.Position.Y);

            // Crude approximation: backstab if attacker is behind target along Y-axis
            return attackerPos.Y < targetPos.Y;
        }

        public void OnLaunchAttack(AttackableUnit targetUnit, bool swag)
        {
            if (!(targetUnit is Champion))
                return;

            bool isDeceive = owner.HasBuff("Deceive");
            bool isBackstab = !isDeceive && IsBehind(owner, targetUnit as ObjAIBase);

            float baseDamage = owner.Stats.AttackDamage.Total;

            if (isDeceive || isBackstab)
            {
                // Guaranteed crit + backstab bonus
                float damage = baseDamage * 2.0f + baseDamage * 0.25f;
                targetUnit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, true);

                if (isDeceive)
                    owner.RemoveBuffsWithName("Deceive");
            }
            else
            {
                targetUnit.TakeDamage(owner, baseDamage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            }
        }

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
