 namespace Buffs
{
    internal class ShacoCOTGPetBuff : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.INTERNAL,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            //Stats applied here might not be notified to the clients, even though all necessary packets are sent, i was unsuccessful on pinpointing the cause (cabeca143).
            if (unit is Pet pet)
            {

                if (owner.HasBuff("Invisibility"))
                {
                    return;
                }
                else
                {
                    AddBuff("Invisibility", 0.70f, 1, ownerSpell, pet, owner);
                    AddBuff("Invisibility", 0.70f, 1, ownerSpell, owner, owner);
                }
            }
        }
    }
}
