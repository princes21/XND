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
            float Damage = data.Damage;
            float AttackerDamage = data.Attacker.Stats.AttackDamage.Total;
            float AttackerAP = data.Attacker.Stats.AbilityPower.Total;
            var turret = data.Target;
            var attacker = data.Attacker;

            if (data.DamageSource == DamageSource.DAMAGE_SOURCE_ATTACK && AttackerAP > AttackerDamage)
            {
                turret.TakeDamage(attacker, AttackerAP, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
            }

            if (data.DamageSource == DamageSource.DAMAGE_SOURCE_ATTACK && data.Attacker is Champion && minutes < 15f)
            {
                {
                    data.PostMitigationDamage = Damage - 45f;
                }
            }
        }
    }
}
