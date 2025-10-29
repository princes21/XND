using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerLib.GameObjects.AttackableUnits;
using System.Numerics;

namespace Spells
{
    public class DariusCleave : ISpellScript
    {
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            NotSingleTargetSpell = true,
            IsDamagingSpell = true,
            TriggersSpellCasts = true
        };
        Spell thisSpell;
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
            ApiEventManager.OnKill.AddListener(this, owner, OnKillChampion, false);
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
			spell.CreateSpellSector(new SectorParameters
            {
                Length = 400f,
                SingleTick = true,
                Type = SectorType.Area,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes | SpellDataFlags.AffectTurrets
            });

        }

        public void OnKillChampion(DeathData deathData)
        {
            var owner = deathData.Killer;
        }
        public void OnSpellPostCast(Spell spell)
        {
			var owner = spell.CastInfo.Owner;
			PlayAnimation(owner, "Spell1", 0.5f);
            AddParticleTarget(owner, owner, "darius_Base_Q_aoe_cast.troy", owner, bone:"C_BuffBone_Glb_Center_Loc");
			AddParticleTarget(owner, owner, "darius_Base_Q_aoe_cast_mist.troy", owner, bone:"C_BuffBone_Glb_Center_Loc");
            AddBuff("AbilityUsed", 4f, 1, spell, owner, owner);
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var spellLevel = owner.GetSpell("DariusCleave").CastInfo.SpellLevel;
            var ADratio = owner.Stats.AttackDamage.Total * 0.35f;
            var damage = 105f + 45f*(spellLevel - 1) + ADratio;
            AddBuff("DariusQSpeedBuff", 2.0f, 1, spell, owner, owner);
            if (target.Team != owner.Team && !(target is ObjBuilding || target is BaseTurret))
			{
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                if (owner.HasBuff("GreviousWound")) {
                    var FloatHeal = 5f;
                    var AdRatio = owner.Stats.AttackDamage.Total * 0.15f;
                    var TrueHeal = FloatHeal + AdRatio;
                    owner.Stats.CurrentHealth += TrueHeal;
                } 
                else
                {
                    var FloatHeal = 25f;
                    var AdRatio = owner.Stats.AttackDamage.Total * 0.40f;
                    var TrueHeal = FloatHeal + AdRatio;
                    owner.Stats.CurrentHealth += TrueHeal;
                }
            
            if (owner.HasBuff("DariusHemoVisual"))
			{
		    AddBuff("DariusHemo", 6.0f, 5, spell, target, owner);
			}
			else
			{
			AddBuff("DariusHemo", 6.0f, 1, spell, target, owner);
			}
            AddParticleTarget(owner, target, "darius_Base_Q_impact_spray.troy", target, 1f);
			AddParticleTarget(owner, target, "darius_Base_Q_tar.troy", target, 1f);
			AddParticleTarget(owner, target, "darius_Base_Q_tar_inner.troy", target, 1f);
			AddParticleTarget(owner, target,"darius_Base_Q_impact_spray.troy", target);
			AddParticleTarget(owner, target, "", target, 1f);
			}
        }
    }
}