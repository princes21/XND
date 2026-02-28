using GameServerCore.NetInfo;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace ItemPassives
{
    public class ItemID_3035 : IItemScript
    {
        AttackableUnit Target;
        Champion Champion;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();


        public void OnActivate(ObjAIBase owner)
        {
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
            StatsModifier.AttackDamage.FlatBonus = 40f;
            owner.AddStatModifier(StatsModifier);
            if (owner.IsMelee && owner is Champion champion)
            {
                Champion.HasABadItem = true;
                var Name = owner.Name;
                ApiFunctionManager.DisplayFloatingText(owner, "You bought a ranged-only item on a melee character. Sell it, you dumbass!", FloatTextType.Absorbed);
                PrintChat($"{Name} bought a ranged-only item on a melee champion. What a dumbass!");
                champion.Die(CreateDeathData(false, 0, owner, owner, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, 0));
            }
        }

        public void OnLaunchAttack(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            Target = spell.CastInfo.Targets[0].Unit;
            if (!owner.IsMelee)
            {
                AddBuff("LastWhisperDebuff", 2f, 1, spell, Target, owner);
                AddBuff("LastWhisperSelfDebuff", 1.5f, 1, spell, owner, owner);
            }
            else if (owner.IsMelee)
            {
                var Name = owner.Name;
                ApiFunctionManager.DisplayFloatingText(owner, "You bought a ranged-only item on a melee character. Sell it, you dumbass!", FloatTextType.Absorbed);
                PrintChat($"{Name} bought a ranged-only item on a melee champion. What a dumbass!");
            }
        }
        public void OnDeactivate(ObjAIBase owner)
        {
            ApiEventManager.OnLaunchAttack.RemoveListener(this);

            if (owner.IsMelee)
            {

            }
        }
    }
}