using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;

namespace Spells
{
    public class DianaVortex : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
			AddParticle(owner, null, "Diana_Base_E_MeshOverlay", owner.Position,10f);
			AddParticle(owner, null, "Diana_Base_E_Precas", owner.Position,10f);
        }

        public void OnSpellPostCast(Spell spell)
        {
			var owner = spell.CastInfo.Owner;
			AddParticle(owner, null, "Diana_Base_E_Cas", owner.Position,10f);
            AddBuff("AbilityUsed", 4f, 1, spell, owner, owner);
            var sector = spell.CreateSpellSector(new SectorParameters
            {
                Length = 450f,
                SingleTick = true,
                Type = SectorType.Area
            });
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var AP = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.60f;
            var AD = spell.CastInfo.Owner.Stats.AttackDamage.Total * 0.80f;
            var damage = 40 + spell.CastInfo.SpellLevel * 30 + AP + AD;
			var dist = System.Math.Abs(Vector2.Distance(target.Position, owner.Position));
			var distt = dist + 125;
			var targetPos = GetPointFromUnit(owner,distt);
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
            AddBuff("Stun", 1.0f, 1, spell, target, owner);
			ForceMovement(target, null, owner.Position, 800, 0, 20, 0);
            AddParticleTarget(owner, target, "Diana_Base_E_Tar", target, 10f);
        }
    }
}
