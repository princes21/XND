using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeaguePackets.Game.Common.CastInfo;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class Headbutt : ISpellScript
    {
        AttackableUnit Target;
        private ObjAIBase _owner;

        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
            // TODO
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
        }
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            _owner = owner;
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }


         public void TargetExecute(Spell spell, AttackableUnit unit, SpellMissile mis, SpellSector sector)
        {
            var Owner = spell.CastInfo.Owner;
            float AP = Owner.Stats.AbilityPower.Total;
            float damage = 125;

            unit.TakeDamage(_owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var AP = owner.Stats.AbilityPower.Total;
            float damage = 50 * spell.CastInfo.SpellLevel + AP;

            AddBuff("AbilityUsed", 4f, 1, spell, owner, owner);
            //ForceMovement(owner, "Spell4", Target.Position, 2200, 0, 0, 0);
            var to = Vector2.Normalize(Target.Position - owner.Position);

            ForceMovement(owner, "Spell2", new Vector2(Target.Position.X - to.X * 100f, Target.Position.Y - to.Y * 100f), 1500, 0, 0, 0);
            AddBuff("Stun", 2.5f, 1, spell, Target, owner);
        }
    }
}
