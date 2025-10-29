using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace Spells
{
    public class DariusBasicAttack : ISpellScript
    {
		private AttackableUnit Target = null;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
			Target = target;
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
			if (owner.HasBuff("DariusNoxianTacticsONH"))
            {
				OverrideAnimation(owner, "Spell2", "Attack1");
			}
			else
			{
				OverrideAnimation(owner, "Attack1", "Spell2");
			}
        }

        public void OnLaunchAttack(Spell spell)
        {
            spell.CastInfo.Owner.SetAutoAttackSpell("DariusBasicAttack2", false);
        }


        public void OnUpdate(float diff)
        {
        }
    }

    public class DariusBasicAttack2 : ISpellScript
    {
		private AttackableUnit Target = null;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true

            // TODO
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
			Target = target;
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
			if (owner.HasBuff("DariusNoxianTacticsONH"))
            {
				OverrideAnimation(owner, "Spell2", "Attack2");
			}
			else
			{
				OverrideAnimation(owner, "Attack2", "Spell2");
			}
        }

        public void OnLaunchAttack(Spell spell)
        {
            spell.CastInfo.Owner.SetAutoAttackSpell("DariusBasicAttack", false);
        }
    }
}
