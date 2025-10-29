using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.StatsNS;

namespace Spells
{
    public class ForcePulse : ISpellScript
    {
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true
            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;

            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            FaceDirection(spellPos, owner, false);

            var sector = spell.CreateSpellSector(new SectorParameters
            {
                Length = 650f,
                SingleTick = true,
                ConeAngle = 39f,
                Type = SectorType.Cone
            });
            AddBuff("AbilityUsed", 4f, 1, spell, owner, owner);

            if (owner.HasBuff("RabadonsDeathcapEffect"))
            {
                var MaxAP = 3000f;
                var MaxHealth = 3250f;
                if (owner.Stats.AbilityPower.Total <= MaxAP)
                {
                    if (owner.Stats.HealthPoints.Total <= MaxHealth)
                    {

                        StatsModifier.AbilityPower.FlatBonus = 14f;
                        owner.AddStatModifier(StatsModifier);
                    }
                    else
                    {
                        owner.Stats.HealthPoints.BaseValue -= 100f;
                    }
                }
            }

            AddParticle(owner, null, "Kassadin_Base_E_cas.troy", owner.Position, direction: owner.Direction);
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;

            var ap = owner.Stats.AbilityPower.Total * 0.8f;
            var damage = 170 + ap;

            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            AddBuff("ForcePulse", 1.5f, 1, spell, target, owner);
        }
    }
}
