using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;


namespace Spells
{
    public class LeonaBasicAttack2 : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };
        AttackableUnit Target;

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
            Target = target;
        }

        public void OnLaunchAttack(Spell spell)
        {
			var owner = spell.CastInfo.Owner;
            var units = GetUnitsInRange(Target.Position, 350f, true);
            Target = spell.CastInfo.Targets[0].Unit;
            float ArMrDamage = owner.Stats.Armor.Total * 0.25f + owner.Stats.MagicResist.Total * 0.40f;

            AddBuff("LeonaIncrementStats", 2.5f, 1, spell, owner, owner);
            AddParticleTarget(owner, Target, "TiamatMelee_itm_hydra.troy", owner);

            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].Team != owner.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                {
                    units[i].TakeDamage(owner, 65f + ArMrDamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                }
            }
        }
    }
}