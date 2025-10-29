using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;

namespace Spells
{
    public class SionW : ISpellScript
    {
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        Spell thisSpell;

        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnKillUnit.AddListener(this, owner, OnKillMinion, false);
            thisSpell = spell;
        }
        public void OnKillMinion(DeathData deathData)
        {
            if (thisSpell.CastInfo.SpellLevel >= 1)
            {
                var owner = deathData.Killer;

                if (owner.Stats.AbilityPower.Total >= 1000)
                {
                    StatsModifier.HealthPoints.FlatBonus = 70;
                    owner.AddStatModifier(StatsModifier);
                    owner.Stats.CurrentHealth += 70;
                }
                else
                {
                    StatsModifier.HealthPoints.FlatBonus = 25;
                    owner.AddStatModifier(StatsModifier);
                    owner.Stats.CurrentHealth += 25;
                }
            }
        }

        public void OnSpellPreCast(ObjAIBase owner,Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            AddBuff("AbilityUsed", 4f, 1, spell, owner, owner);
            AddBuff("SionMixedShield", 2f, 1, spell, owner, owner);
            if (owner.HasBuff("GreviousWound"))
            {
                owner.Stats.CurrentHealth += owner.Stats.HealthPoints.Total * 0.10f;
            }
            else
            {
                owner.Stats.CurrentHealth += owner.Stats.CurrentHealth += owner.Stats.HealthPoints.Total * 0.35f;
            }
        }
    }
}

