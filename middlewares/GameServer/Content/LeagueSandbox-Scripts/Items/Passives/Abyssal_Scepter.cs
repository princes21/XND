using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace ItemPassives
{
    public class ItemID_3001 : IItemScript
    {
        AttackableUnit Target;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(ObjAIBase owner)
        {
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
            StatsModifier.ManaPoints.FlatBonus = 300f;
            StatsModifier.Armor.FlatBonus = 40f;
            StatsModifier.AbilityPower.FlatBonus = 30f;
            owner.AddStatModifier(StatsModifier);
            owner.Stats.CurrentMana += 300f;
        }
        public void OnLaunchAttack(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            Target = spell.CastInfo.Targets[0].Unit;
            if (owner.Stats.Range.Total <= 255)
            {
                AddBuff("AbyssalScepterDebuff", 3.5f, 1, spell, Target, owner);
                AddBuff("AbyssalScepterBuff", 8.0f, 1, spell, owner, owner);
            }
        }
        public void OnDeactivate(ObjAIBase owner)
        {
            ApiEventManager.OnLaunchAttack.RemoveListener(this);
        }
    }
}