using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerLib.GameObjects.AttackableUnits;

namespace ItemPassives
{
    public class ItemID_3143 : IItemScript
    {
        AttackableUnit Target;
        private ObjAIBase owner;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(ObjAIBase owner)
        {
            this.owner = owner;
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
            ApiEventManager.OnTakeDamage.AddListener(this, owner, OnHitByEnemy, false);

            StatsModifier.Armor.FlatBonus = 30f;
            //StatsModifier.Armor.FlatBonus = 30f;
            owner.AddStatModifier(StatsModifier);
        }
        public void OnLaunchAttack(Spell spell)
        {
            //var owner = spell.CastInfo.Owner;
            Target = spell.CastInfo.Targets[0].Unit;
            AddBuff("RanduinsOmenSlow", 2f, 1, spell, Target, owner);
        }

        private void OnHitByEnemy(DamageData data)
        {
            AddBuff("RanduinsRegenBuff", 3.0f, 1, null, owner, owner);
        }
        public void OnDeactivate(ObjAIBase owner)
        {
            ApiEventManager.OnLaunchAttack.RemoveListener(this);
            ApiEventManager.OnTakeDamage.RemoveListener(this);
        }
    }
}