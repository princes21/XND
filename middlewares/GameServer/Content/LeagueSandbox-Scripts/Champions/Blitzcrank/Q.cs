using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using System.Reflection;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class RocketGrab : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
            // TODO
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            FaceDirection(end, owner);
        }

        ObjAIBase Owner;
        AttackableUnit Unit;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Owner = owner;
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
            ApiEventManager.OnSpellCast.AddListener(this, owner.GetSpell("Q"), OnSpellCast);

        }
        Particle p;

        public void OnSpellCast(Spell spell)
        {
            // Prevent flash during cast.
            SealSpellSlot(spell.CastInfo.Owner, SpellSlotType.SummonerSpellSlots, 0, SpellbookType.SPELLBOOK_SUMMONER, true);
            SealSpellSlot(spell.CastInfo.Owner, SpellSlotType.SummonerSpellSlots, 1, SpellbookType.SPELLBOOK_SUMMONER, true);
            PlayAnimation(Owner, "Spell1");
            Owner.StopMovement();
            Owner.SetStatus(StatusFlags.CanMove, false);

        }

        public void OnSpellPostCast(Spell spell)
        {
            Owner.SetStatus(StatusFlags.CanMove, true);
            RemoveParticle(p);
            var owner = spell.CastInfo.Owner;
            AddBuff("AbilityUsed", 4f, 1, spell, owner, owner);
            SealSpellSlot(spell.CastInfo.Owner, SpellSlotType.SummonerSpellSlots, 0, SpellbookType.SPELLBOOK_SUMMONER, false);
            SealSpellSlot(spell.CastInfo.Owner, SpellSlotType.SummonerSpellSlots, 1, SpellbookType.SPELLBOOK_SUMMONER, false);
            var sector = spell.CreateSpellSector(new SectorParameters
            {
                Length = 1150f,
                SingleTick = true,
                ConeAngle = 15.76f,
                Type = SectorType.Cone
            });

            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var ap = owner.Stats.AbilityPower.Total;
            var damage = 80 + ((spell.CastInfo.SpellLevel - 1) * 55) + ap;
            var dist = System.Math.Abs(Vector2.Distance(target.Position, owner.Position));
            var time = dist / 1350f;

            // Grab particle
            AddBuff("RocketGrab", time, 1, spell, target, owner);

            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);

            AddBuff("Stun", 0.6f, 1, spell, target, owner);
            if (missile != null)
            {
                missile.SetToRemove();
            }

        }
    }
}


