using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;

namespace Spells
{
    public class KatarinaR : ISpellScript
    {

        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            NotSingleTargetSpell = true,
            TriggersSpellCasts = true,
            ChannelDuration = 2.5f,
        };

        private Vector2 basepos;
        public SpellSector DamageSector;
        ObjAIBase Owner;
        public void OnSpellChannel(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            AddBuff("AbilityUsed", 4f, 1, spell, owner, owner);
            AddBuff("KatarinaR", 2.5f, 1, spell, owner, owner);
        }

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource reason)
        {
			var owner = spell.CastInfo.Owner;
            owner.RemoveBuffsWithName("KatarinaR");
        }
    }

    public class KatarinaRMis : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target
            }
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
			ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
			var owner = spell.CastInfo.Owner;
            var AP = owner.Stats.AbilityPower.Total * 0.35f;
            var AD = owner.Stats.AttackDamage.FlatBonus * 0.50f;
            float damage = 45f + ( 95f * spell.CastInfo.SpellLevel) + AP + AD;
			target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
            AddBuff("GreviousWound", 9.5f, 1, spell, target, owner);
            owner.GetSpell("KatarinaQ").SetCooldown(0.00f);
            owner.GetSpell("KatarinaW").SetCooldown(0.00f);
            owner.GetSpell("KatarinaE").SetCooldown(0.00f);
			AddParticleTarget(owner, target, "katarina_deathLotus_tar.troy", target);         	
        }
    }
}