using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace ItemPassives
{
    public class ItemID_3071 : IItemScript
    {
        AttackableUnit Target;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(ObjAIBase owner)
        {
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
            StatsModifier.HealthPoints.FlatBonus = 600f;
            StatsModifier.CooldownReduction.FlatBonus -= 0.10f;
            owner.AddStatModifier(StatsModifier);
            owner.Stats.CurrentHealth += 600f;
        }
        public void OnLaunchAttack(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            Target = spell.CastInfo.Targets[0].Unit;
            if (owner.Stats.Range.Total <= 255)
            {
                AddBuff("BlackCleaver", 2f, 1, spell, Target, owner);
                AddBuff("TheBlackCleaverSelfBuff", 5f, 1, spell, owner, owner);
            }
        }
        public void OnDeactivate(ObjAIBase owner)
        {
            ApiEventManager.OnLaunchAttack.RemoveListener(this);
        }
    }
}