using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace ItemPassives
{
    public class ItemID_1501 : IItemScript
    {
        ObjAIBase owner;
        private Buff sourceBuff;
        DamageData _damageData;
        AttackableUnit Attacker;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();



        public void OnActivate(ObjAIBase owner)
        {
            this.owner = owner;
            StatsModifier.Armor.FlatBonus = 20f;
            StatsModifier.MagicResist.FlatBonus = 25f;
            owner.AddStatModifier(StatsModifier);
            ApiEventManager.OnPreTakeDamage.AddListener(this, owner, OnPreTakeDamage, false);

        }

        private void OnPreTakeDamage(DamageData data)
        {
            float minutes = GameTime() / (1000f * 60f);

            if (data.DamageSource == DamageSource.DAMAGE_SOURCE_ATTACK && data.Attacker is Champion)
            {
                data.PostMitigationDamage -= 90f;
                
            }
        }
    }
}
